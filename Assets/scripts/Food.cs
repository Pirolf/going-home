using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Food : MonoBehaviour {
	public string name;
	public float price;
	public int total;
	public int hunger; //how much 1 serving can replenish player's hunger
	public int happiness;
	public int healthScore;

	public int index = -1; //food index in the inventory, < 0 means not in inventory
	public int uiIndex = -1;
	public int quantity = 0; //amount of food in foods[] inventory
	public Texture texture;

	public GameObject totalText;
	public GameObject quantityText;
	public GameObject priceText;

	[HideInInspector]public Text totalTextComp;
	[HideInInspector]public Text quantityTextComp;
	[HideInInspector]public Text priceTextComp;

	public void OnEat(GameObject foodUIContainer){
		PlayerData pd = PlayerData.playerData;
		pd.hunger += this.hunger;
		pd.accumultiveHealth += this.healthScore;
		pd.happiness += this.happiness;
		pd.RemoveFoodFromInventory(1, foodUIContainer);
		//update stats
		pd.UpdateStatsDisplay();
	}
	void Awake(){
		totalTextComp = totalText.GetComponent<Text>();
		quantityTextComp = quantityText.GetComponent<Text>();
		priceTextComp = priceText.GetComponent<Text>();
		
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
