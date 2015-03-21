using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PlayerData : MonoBehaviour {
	public static PlayerData playerData;
	public static GameObject playerDataCanvas;
	public GameObject healthText;
	public GameObject hungerText;
	public GameObject happinessText;
	public GameObject moneyText;
	public GameObject inventoryPanel;
	public GameObject inventoryBtn;

	private Text healthTextComp;
	private Text hungerTextComp;
	private Text happinessTextComp;
	private Text moneyTextComp;
	private Button inventoryBtnComp;
	public GameObject[] foodSlotsUI;

	public int health; // 0- 100
	public int hunger; // 0- 10
	public int happiness;// 0 -10

	public int accumultiveHealth; //negative: if eat harmful food, getting cold
	
	//public int warmth; 
	public float money; 
	public Transform location;
	public int sceneIdx;
	public int day;
	public float hour;

	//inventory
	public Food[] foods;


	public void UpdateStatsDisplay(){
		healthTextComp.text = health+"";
		hungerTextComp.text = hunger+"";
		happinessTextComp.text = happiness+"";
		moneyTextComp.text = money+"";
	}
	public void OnClickFoodItemInInventory(GameObject foodUIContainer){
		Debug.Log("click food!");
		Food f = foods[ int.Parse(foodUIContainer.name)];
		if(f == null)return;
		f.OnEat(foodUIContainer);
	}
	public void RemoveFoodFromInventory(int quantity, GameObject foodUIContainer
		, bool removeAll = false){
		//get fooduicontainer name eg name = 0
		//find food in foods[] with index 0
		if(quantity <= 0 && !removeAll)return;
		Food f = foods[ int.Parse(foodUIContainer.name)];
		if(f == null)return;
		f.quantity -= quantity;
		GameObject foodUI = foodSlotsUI[f.uiIndex];

		foodUI.transform.Find("quantity").gameObject
			.GetComponent<Text>().text = f.quantity+"";

		if(f.quantity <= 0){
			//delete raw image
			foodUI.transform.Find("sprite").gameObject
				.GetComponent<Image>().sprite = null;
			//delete currFood
			string foodName = f.gameObject.name;

			Destroy(f.gameObject);
		}

	}
	public void AddFoodToInventory(int quantity, ref Food f){
		if(quantity <= 0)return;
		Food foodToUpdate = null;
		if(f.index < 0){
			//not in inventory yet
			int i = 0;
			for( i=0; i < foods.Length; i++){
				if(foods[i] == null){
					//insert
					foods[i] = Instantiate(f, Vector3.zero, Quaternion.identity) as Food;
					//foods[i].transform.parent = GameObject.Find("homelessPlayer").transform;
					foods[i].index = i;
					foods[i].quantity = quantity;
					
					foodToUpdate = foods[i];

					f = foodToUpdate;
					break;
				}
			}
			//food inventory is full!
			if(i >= foods.Length)return;
		}else{
			//food is already in inventory
			Food foodOwned = foods[f.index];
			foodOwned.quantity += quantity;
			foodToUpdate = foodOwned;

		}
		//food[0] stored at foodslotsui named 0
		//update food text
		for(int i=0; i<foodSlotsUI.Length;i++){
			GameObject curr = foodSlotsUI[i];
			Debug.Log(curr.name);

			if(curr.name.Equals(f.index + "")){
				//update raw img
				GameObject img = curr.transform.Find("sprite").gameObject;
				img.GetComponent<Image>().sprite = foodToUpdate.icon;
				//upadte quantity display
				GameObject quantityText = curr.transform.Find("quantity").gameObject;
				if(quantityText == null){
					Debug.Log("no text");
					return;
				}
				foodToUpdate.uiIndex = i;
				quantityText.GetComponent<Text>().text = foodToUpdate.quantity + "";
			}
		}
	}
	public void UpdateMoneyText(){
		moneyTextComp.text = money + "";
	}
	public void OnClickInventoryBtn(){
		inventoryPanel.SetActive(true);
		//disable open inventory btn
		inventoryBtnComp.interactable = false;
		//disable mouse look
		gameObject.GetComponent<MouseLook>().enabled = false;
		Camera.main.gameObject.GetComponent<MouseLook>().enabled = false;

	}
	public void OnClickCloseInventoryBtn(){
		Debug.Log("close");
		inventoryPanel.SetActive(false);
		//enable open inventory btn
		inventoryBtnComp.interactable = true;
		
		
	}
	void Awake(){
		playerData = this;
		
		foods = new Food[7];
		foodSlotsUI = GameObject.FindGameObjectsWithTag("Food");

		playerDataCanvas = GameObject.Find("PlayerDataCanvas");
		healthTextComp = healthText.GetComponent<Text>();
		hungerTextComp = hungerText.GetComponent<Text>();
		happinessTextComp = happinessText.GetComponent<Text>();
		moneyTextComp = moneyText.GetComponent<Text>();

		inventoryBtnComp = inventoryBtn.GetComponent<Button>();
		inventoryPanel.SetActive(false);
	}
	void Start () {
		health = 80;
		hunger = 5;
		happiness = 3;
		accumultiveHealth = 0;
		money = 10;

		healthTextComp.text = health + "";
		hungerTextComp.text = hunger + "";
		happinessTextComp.text = happiness + "";
		moneyTextComp.text = money + "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
