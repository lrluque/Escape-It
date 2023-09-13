using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBreathing : MonoBehaviour
{

    [Header("Breathing")]
    public AudioSource runningBreathingAudio;
    public AudioSource normalBreathingAudio;
    public GameObject Monster;

    //We create an array with 2 audio sources that we will swap between for transitions
    public static AudioSource[] aud = new AudioSource[2];
    //We will use this boolean to determine which audio source is the current one
    bool activeMusicSource;
    //We will store the transition as a Coroutine so that we have the ability to stop it halfway if necessary
    IEnumerator musicTransition;

    void Awake() {
        aud[0] = normalBreathingAudio;
        aud[1] = runningBreathingAudio;
    }

   // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Check if chasing player
        if (Monster.GetComponent<GOAPPlanner>().ActiveAction.GetType().ToString() == "Action_Chase")
        {
            if (!runningBreathingAudio.isPlaying)
            {
                newSoundtrack(runningBreathingAudio.clip);
            }
        }
        else
        {
            if (!normalBreathingAudio.isPlaying)
            {
                newSoundtrack(normalBreathingAudio.clip);
            }
        }
    }

    public void newSoundtrack (AudioClip clip) {
        //This ?: operator is short hand for an if/else statement, eg.
        //
        //      if (activeMusicSource) {
        //          nextSource = 1;
        //      } else {
        //           nextSource = 0;
        //      }
 
        int nextSource = !activeMusicSource ? 0 : 1;
        int currentSource = activeMusicSource ? 0 : 1;
 
        //If the clip is already being played on the current audio source, we will end now and prevent the transition
        if (clip == aud[currentSource].clip)
            return;
 
        //If a transition is already happening, we stop it here to prevent our new Coroutine from competing
        if (musicTransition != null)
            StopCoroutine(musicTransition);
 
        aud[nextSource].clip = clip;
        aud[nextSource].Play();
 
        musicTransition = transition(15); //20 is the equivalent to 2 seconds (More than 3 seconds begins to overlap for a bit too long)
        StartCoroutine(musicTransition);
    }
 
        //  'transitionDuration' is how many tenths of a second it will take, eg, 10 would be equal to 1 second
    IEnumerator transition(int transitionDuration) {
 
        for (int i = 0; i < transitionDuration+1; i++) {
            aud[0].volume = activeMusicSource ? (transitionDuration - i) * (1f / transitionDuration) : (0 + i) * (1f / transitionDuration);
            aud[1].volume = !activeMusicSource ? (transitionDuration - i) * (1f / transitionDuration) : (0 + i) * (1f / transitionDuration);
            aud[0].volume = Mathf.Clamp(aud[0].volume, 0f, 0.6f);
            aud[1].volume = Mathf.Clamp(aud[1].volume, 0f, 0.6f);
 
            yield return new WaitForSecondsRealtime(0.1f);
            //use realtime otherwise if you pause the game you could pause the transition half way
        }
 
        //finish by stopping the audio clip on the now silent audio source
        aud[activeMusicSource ? 0 : 1].Stop();
 
        activeMusicSource = !activeMusicSource;
        musicTransition = null;
    }
}
