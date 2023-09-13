using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
[SerializeField] private Animator _rightHandAnimator;
[SerializeField] private Animator _leftHandAnimator;

public GameObject Player;

void Awake()
{
}

void Update()
{
    if (Player.GetComponent<FirstPersonMovement>().IsRunning)
    {
        _rightHandAnimator.SetBool("Running", true);
        _rightHandAnimator.SetBool("Walking", false);
        _leftHandAnimator.SetBool("Running", true);
        _leftHandAnimator.SetBool("Walking", false);
    }
    else if (Player.GetComponent<FirstPersonMovement>().IsMoving)
    {
        _rightHandAnimator.SetBool("Running", false);
        _rightHandAnimator.SetBool("Walking", true);
        _leftHandAnimator.SetBool("Running", false);
        _leftHandAnimator.SetBool("Walking", true);
    }
    else
    {
        _rightHandAnimator.SetBool("Running", false);
        _rightHandAnimator.SetBool("Walking", false);
        _leftHandAnimator.SetBool("Running", false);
        _leftHandAnimator.SetBool("Walking", false);
    }
}


}
