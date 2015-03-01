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
	public List<Food> menu;
	public Food friedChicken;
	public Food coke;
	public Food cheeseBurger;

	public Vector3 playerSpawnLoc = new Vector3(915.1311f,2.272144f,170.1751f);
	void OnCollisionEnter(Collision collision){
		Debug.Log("hit!");
		GameControl.colliderPos = gameObject.transform.position;
		//save location
		GameControl.gameControl.SaveLocInMain();
		//switch to restaurant scene
		//Application.LoadLevel("fastfoodShop");
		//go to restaurant
		GameControl.player.transform.position = playerSpawnLoc;
		GameControl.player.transform.rotation = Quaternion.identity;
		Camera.main.gameObject.transform.rotation = Quaternion.identity;
		GameControl.player.GetComponent<PlayerController>().EnableMouseLook(false);
		//enable fast food menu
		fastfoodMenuCanvas.SetActive(true);
		leaveCanvas.SetActive(true);
		GameControl.player.GetComponent<PlayerController>().disableMove = true;
	}
	public void OnLeave(){
		GameControl.player.GetComponent<PlayerController>().disableMove = false;
		fastfoodMenuCanvas.SetActive(false);
	}
	
	void Awake(){
		restaurant = this;
		fastfoodMenuCanvas.SetActive(false);
		leaveCanvas.SetActive(false);
		menu = new List<Food>();
		//add foods to menu
		if(friedChicken == null || coke == null ||  cheeseBurger == null){
			Debug.Log("some food is null!");
			return;
		}
		menu.Add(friedChicken);
		menu.Add(cheeseBurger);
		menu.Add(coke);

		//rand quantities
		foreach(Food f in menu){
			int q = Mathf.FloorToInt(Random.Range(0, 10));
			f.total = q;
			f.totalTextComp.text = q + "";
			f.priceTextComp.text = f.price + "";
		}
	}

	void Start(){

	}
	void Update(){

	}
}