using UnityEngine;
using System.Collections;

public class NPC_NavMeshAgent : MonoBehaviour {
	private NavMeshAgent nma;
	public Transform target;
	// Use this for initialization
	void Start () {
		nma = gameObject.GetComponent<NavMeshAgent>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameControl.gameState == (int) GameControl.GameState.PlayerNavigating){

			nma.SetDestination(target.position);
			nma.Resume();
		}else{
			nma.Stop();
		}
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
	}
}
