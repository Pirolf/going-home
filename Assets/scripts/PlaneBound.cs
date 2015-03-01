using UnityEngine;
using System.Collections;

public class PlaneBound : MonoBehaviour{
	void Awake(){

	}
	void Start(){

	}
	void OnTriggerEnter(Collider other){
		//if NPC
		if(other.gameObject.tag.Equals("NPC")){
			NPC npc = other.gameObject.GetComponent<NPC>();
			npc.currentDirection = Intersection.GetOppositeDirection(npc.currentDirection);
			//reset personality
			npc.InitPersonality();
		}
	}
}
