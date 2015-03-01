using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intersection : MonoBehaviour {
	public int[] directions = new int[4];
	public List<int> tempDirs = new List<int>();
	[HideInInspector]public static Vector3 facePosZ = new Vector3(270,-90,0);
	[HideInInspector]public static Vector3 faceNegZ = new Vector3(270,90,0);
	[HideInInspector]public static Vector3 facePosX = new Vector3(270,0,0);
	[HideInInspector]public static Vector3 faceNegX = new Vector3(270,180,0);
	//value 0: posz 
	//1: negz
	//2: posx
	//3: negx
	void Awake(){

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionStay(){
		Debug.Log("collision enter");
	}
	public static int GetOppositeDirection(int d){
		if(d == 0){
			return 1;
		}
		if(d == 1){
			return 0;
		}
		if(d ==2){
			return 3;
		}
		if(d==3){
			return 2;
		}
		return 0;
	}
	void OnTriggerEnter(Collider other){
		if(!other.gameObject.tag.Equals("NPC"))return;
		
		NPC npc = other.gameObject.GetComponent<NPC>();
		tempDirs.Clear();
		for(int i=0; i <directions.Length;i++){
			int curr = directions[i];
			if(curr != -1 && curr != GetOppositeDirection(npc.currentDirection)){
				tempDirs.Add(curr);
			}
		}
		int r = Mathf.FloorToInt(UnityEngine.Random.Range(0, tempDirs.Count));
		//npc.currentDirection = tempDirs[r];
		npc.nextDirection = tempDirs[r];
		npc.nextCheckPoint = transform.position;
		//Debug.Log(npc.nextCheckPoint);
		if(npc.nextDirection == 0){
			//pos z
			npc.nextTurnDegree = facePosZ;
		}else if(npc.nextDirection == 1){
			npc.nextTurnDegree = faceNegZ;
		}else if(npc.nextDirection == 2){
			//pos x
			npc.nextTurnDegree = facePosX; 
		}else if(npc.nextDirection == 3){
			npc.nextTurnDegree = faceNegX;
		}
		//turn
	}

}
