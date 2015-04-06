using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour {

	public float temper;
	public float courage;
	public bool beggedToday = false;

	private Renderer rend;
	private NavMeshAgent nma;
	private Animation anim;
	// Use this for initialization
	void Start () {
		rend = transform.Find("NPCBody").gameObject.GetComponent<Renderer>();
		nma = transform.parent.GetComponent<NavMeshAgent>();
		//haven't talked with player, set to 99 (player can collide with npc and talk)
		//after talking to player today, set to 0 to avoid talking to player again
		nma.avoidancePriority = 80;
		anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameControl.gameState == (int)GameControl.GameState.PlayerNavigating){
			//anim.Play("walk");
			anim.Blend("walk");
		}else{
			anim.Stop();
		}
	}
	void OnEnable(){
		InitPersonality();
		//Debug.Log(temper);
	}
	public void InitPersonality(){
		temper = UnityEngine.Random.Range(0,10) / 10.0f;
		courage = UnityEngine.Random.Range(0,1);
	}
	/*
	void OnMouseDown(){
		//if(transform.parent.gameObject != PlayerController.self.MouseOverObject){
		//	return;
		//}
		PlayerController pc = PlayerController.self;
		float distPlayerAndMe = Vector3.Distance(transform.position, 
			pc.gameObject.transform.position);
		Debug.Log("dist: " + distPlayerAndMe);
		if(distPlayerAndMe > pc.MaxNPCRaycastDistance){
			return;
		}
		Debug.Log("player clicked on NPC");
	}
	*/
	/*
	void OnMouseEnter(){
		//change cursor
		Debug.Log("mouse entered me!");
	}
	*/
	/*
	void OnCollisionEnter(Collision other){
		if(other.gameObject.name.Equals("homelessPlayer")){
			
			GameControl.player.GetComponent<PlayerController>().disableMove = true;
			GameControl.gameState = (int)GameControl.GameState.HumanInteraction;
			
			beggedToday = true;
			//look at each other in the eyes...
			Vector3 npcEyes = GetEyePosition(rend);

			Vector3 playerEyes = GetEyePosition(
				GameControl.player.GetComponent<PlayerController>()
				.playerBody.GetComponent<MeshRenderer>());
//			Debug.Log(playerEyes);
			Camera.main.gameObject.transform.LookAt(transform);
			gameObject.transform.parent.transform.LookAt(Camera.main.transform);

			DialogueSystem.dialogueSystem.StartInteraction(temper, beggedToday);

			//GameControl.player.GetComponent<NavMeshObstacle>().enabled = true;
			//nma.avoidancePriority = 0;
		}
	}
	*/
	public Vector3 GetEyePosition(Renderer mr){
		Vector3 myCenter = mr.bounds.center;
		Vector3 myExtents = mr.bounds.extents;
		Vector3 myEyes = myCenter;
		myEyes.z += myExtents.y * 0.6f;
		return myEyes;
	}
}
