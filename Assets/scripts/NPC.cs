using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
	public int currentDirection;
	public int nextDirection;
	public Vector3 nextTurnDegree;
	public float speed;
	public float rotSpeed;
	public Vector3 nextCheckPoint; //for turning
	public bool interrupted = false;

	public float temper;
	public float courage;
	public bool beggedToday = false;
	// Use this for initialization
	void Awake(){
		nextCheckPoint = Vector3.zero;
		nextDirection = -1;
		nextTurnDegree = gameObject.transform.rotation.eulerAngles;
		speed = 1.5f;
		rotSpeed = 0.5f;
		
	}
	void OnEnable(){
		InitPersonality();
		Debug.Log(temper);
	}
	public void InitPersonality(){
		temper = UnityEngine.Random.Range(0,10) / 10.0f;
		courage = UnityEngine.Random.Range(0,1);
	}
	void Start () {
		if(gameObject.name.Equals("NPC_rigged")){
			Animation anim = gameObject.GetComponent<Animation>();
			foreach (AnimationState state in anim) {
            	state.speed = 4F;
        	}
		}
		StartCoroutine(NPCMove());
	}
	void Update(){
		if(gameObject.name.Equals("NPC_rigged")){
			Animation anim = gameObject.GetComponent<Animation>();
			if(GameControl.gameState != (int)GameControl.GameState.HumanInteraction){
				
				foreach (AnimationState state in anim) {
            		state.speed = 4F;
        		}
        	}else{
        		foreach (AnimationState state in anim) {
            		state.speed = 0F;
        		}
        	}
		}
	}
	IEnumerator NPCMove(){
		while(true){
			yield return new WaitForSeconds(1f / 30f);
			if(GameControl.gameState == (int) GameControl.GameState.PlayerNavigating){
	        	FollowDirection();
	        }
		}
	}

	public void FollowDirection(){
		float delta = speed * Time.deltaTime;

		//Vector3 childEuler = childNPC.transform.eulerAngles;
		if(nextDirection == 0 || nextDirection == 1){
			//z
			if(Mathf.Abs(transform.position.x -nextCheckPoint.x) < 0.05){
				transform.eulerAngles = nextTurnDegree;
				currentDirection = nextDirection;
				nextDirection = -1;
				nextCheckPoint = Vector3.zero;
			}
		}else if (nextDirection == 2 || nextDirection == 3){
			//x
			if(Mathf.Abs(transform.position.z -nextCheckPoint.z)< 0.05){
				transform.eulerAngles = nextTurnDegree;
				currentDirection = nextDirection;
				nextDirection = -1;
				nextCheckPoint = Vector3.zero;
			}
		}
        
        
        if(interrupted)return;
        Vector3 oldPos = transform.position;
		if(currentDirection == 0){
			transform.position += Vector3.forward * delta;
		}else if(currentDirection ==1){
			transform.position += Vector3.back * delta;
		}else if(currentDirection ==2){
			transform.position += Vector3.right * delta;
		}else if(currentDirection == 3){
			transform.position += Vector3.left * delta;
		}
		

	}
	
	void OnCollisionEnter(Collision other){
		Debug.Log("npc collides player");
		if(other.gameObject.name.Equals("homelessPlayer")){
			
			GameControl.player.GetComponent<PlayerController>().disableMove = true;
			GameControl.gameState = (int)GameControl.GameState.HumanInteraction;
			Debug.Log(temper);
			beggedToday = true;
			DialogueSystem.dialogueSystem.StartInteraction(temper, beggedToday);
			
		}
	}


	
}
