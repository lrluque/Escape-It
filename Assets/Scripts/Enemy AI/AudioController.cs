using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    // List of foosteps sounds
    [Header("Footsteps")]
    public List<AudioClip> footsteps = new List<AudioClip>();
    private AudioSource _audioSource;
    


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Footstep()
    {
        // Play a random footstep sound
        
        _audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Count)]);
    }
}
