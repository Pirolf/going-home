using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Restaurant : MonoBehaviour{
	//menu: list of food
	//food: name, price, hunger, quantity
	public static Restaurant restaurant;
	public GameObject fastfoodMenuCanvas;
	public GameObject leaveCanvas;
	public GameObject conversationLoc;

	public List<Food> menu = new List<Food>();
	public Food friedChicken;
	public Food coke;
	public Food cheeseBurger;

	
	//return to main scene
	public void OnLeave(){
		PlayerController pc = GameControl.player.GetComponent<PlayerController>();

		leaveCanvas.SetActive(false);
		pc.EnableMouseLook(true);
		pc.disableMove = false;
		
	}
	void OnCollisionEnter(Collision other){
		Debug.Log("player enter fastfood shop");
		PlayerController pc = GameControl.player.GetComponent<PlayerController>();
		Vector3 playerPos = GameControl.player.transform.position;
		pc.EnableMouseLook(false);
		pc.disableMove = true;

		leaveCanvas.SetActive(true);
		GameControl.player.transform.position = new Vector3(
			conversationLoc.transform.position.x,
			playerPos.y, 
			conversationLoc.transform.position.z);

		GameControl.player.transform.LookAt(new Vector3(
			gameObject.transform.position.x, 
			GameControl.player.transform.position.y,
			gameObject.transform.position.z));

		pc.playerCam.transform.localEulerAngles = Vector3.zero;

		
		
	}
	void OnCollisionStay(Collision other){
		GameControl.player.transform.position = new Vector3(conversationLoc.transform.position.x,
			GameControl.player.transform.position.y, GameControl.player.transform.position.z);
	}
	void OnTriggerExit(Collider other){
		Debug.Log("player exit fast food shop");
		//GameControl.player.GetComponent<PlayerController>().EnableMouseLook(true);
		//leaveCanvas.SetActive(false);
	}
	
	
	void Awake(){
		restaurant = this;
		//fastfoodMenuCanvas.SetActive(false);
		leaveCanvas.SetActive(false);
		//add foods to menu
		if(friedChicken == null || coke == null ||  cheeseBurger == null){
			Debug.Log("some food is null!");
			return;
		}
		/*
		menu.Add(friedChicken);
		menu.Add(cheeseBurger);
		menu.Add(coke);
		*/
		//rand quantities
		
	}

	void Start(){
		foreach(Food f in menu){
			int q = Mathf.FloorToInt(Random.Range(0, 10));
			f.total = q;
			//Debug.Log("food: " + q);
			f.totalTextComp.text = q + "";
			f.priceTextComp.text = f.price + "";
		}
	}
	void Update(){

	}
}