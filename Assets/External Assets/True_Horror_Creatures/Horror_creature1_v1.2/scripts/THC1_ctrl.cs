using UnityEngine;
using System.Collections;

public class THC1_ctrl : MonoBehaviour {
	
	
	private Animator anim;

	public GameObject Body;

	private CharacterAgent Agent;
	private UnityEngine.AI.NavMeshAgent nav;
	
	[SerializeField] private int battle_state = 0;
	[SerializeField] public float speed = 6.0f;
	[SerializeField] public float runSpeed = 3.0f;
	[SerializeField] public float turnSpeed = 60.0f;
	[SerializeField] public float gravity = 20.0f;
	[SerializeField] private Vector3 moveDirection = Vector3.zero;
	[SerializeField] private float w_sp = 1.6f;
	[SerializeField] private float r_sp = 3;

	
	// Use this for initialization
	void Start () 
	{						
		anim = Body.GetComponent<Animator>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		Agent = GetComponent<CharacterAgent>();
	}
	
	// Update is called once per frame
	void Update () 
	{		

		if (Agent.isWalking)
		{
			anim.SetBool ("walking", true);
			anim.SetBool ("running", false);
			nav.speed = 3.0f;
			Debug.Log("walking");
		}else if (Agent.IsRunning)
		{
			anim.SetBool ("running", true);
			anim.SetBool ("walking", false);
			nav.speed = 6.0f;
			Debug.Log("running");
		}
		else
		{
			anim.SetBool ("walking", false);
			anim.SetBool ("running", false);
			nav.speed = 0.0f;
			Debug.Log("idle");
		}
	}
}



