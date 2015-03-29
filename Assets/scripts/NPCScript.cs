using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour {

	public float temper;
	public float courage;
	public bool beggedToday = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnEnable(){
		InitPersonality();
		Debug.Log(temper);
	}
	public void InitPersonality(){
		temper = UnityEngine.Random.Range(0,10) / 10.0f;
		courage = UnityEngine.Random.Range(0,1);
	}
	void OnCollisionEnter(Collision other){
		Debug.Log("npc collides player");
		if(other.gameObject.name.Equals("homelessPlayer")){
			
			GameControl.player.GetComponent<PlayerController>().disableMove = true;
			GameControl.gameState = (int)GameControl.GameState.HumanInteraction;
			
			beggedToday = true;
			DialogueSystem.dialogueSystem.StartInteraction(temper, beggedToday);
			
		}
	}
}
