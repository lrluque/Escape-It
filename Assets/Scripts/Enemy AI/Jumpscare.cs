using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Jumpscare : MonoBehaviour
{
    public string scenename;
    public float jumpscareTimer;
    public AudioSource jumpScareSound;
    public GameObject jumpScareCam;
    public GameObject player;

    public Animator anim;
 
    void Start()
    {
        jumpScareCam.SetActive(false);
    }
 
    public IEnumerator JumpScarePlayer()
    {
        jumpScareCam.SetActive(true);
        player.SetActive(false);
        yield return new WaitForSeconds(jumpscareTimer);
        SceneManager.LoadScene(scenename);
    }
}