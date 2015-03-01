using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FastFoodMenu : MonoBehaviour {
	private int friedChickenQ = 0;
	private int cheeseBurgerQ = 0;
	private int cokeQ = 0;


	public GameObject totalChargeText;
	private float totalCharge;
	private Text totalChargeTextComp;
	private Color moneyNotEnough = new Color(0.8f, 0.2f, 0.3f,1.0f);
	private Color normalTextColor = new Color(50/255.0f, 50/255.0f, 50/255.0f, 1.0f);
	void Awake(){
		totalChargeTextComp = totalChargeText.GetComponent<Text>();
	}
	public void UpdateTotal(){
		float total = 0.0f;
		Restaurant r  = Restaurant.restaurant;
		total = r.friedChicken.price * friedChickenQ 
			+ r.cheeseBurger.price * cheeseBurgerQ
			+ r.coke.price * cokeQ;
		totalCharge = total;
		totalChargeTextComp.text = "Total: " + total;
		if(PlayerData.playerData.money < totalCharge){
			totalChargeTextComp.color = moneyNotEnough;
		}else{
			totalChargeTextComp.color = normalTextColor;
		}
	}
	public void OnCheckout(){
		if(PlayerData.playerData.money < totalCharge)return;
		PlayerData.playerData.money -= totalCharge;
		//update money text
		PlayerData.playerData.UpdateMoneyText();
		//add food to inventory
		
		Restaurant r = Restaurant.restaurant;
		PlayerData.playerData.AddFoodToInventory(friedChickenQ, ref r.friedChicken);
		PlayerData.playerData.AddFoodToInventory(cheeseBurgerQ, ref r.cheeseBurger);
		PlayerData.playerData.AddFoodToInventory(cokeQ, ref r.coke);
	

	}
	public void OnAddFoodQuantity(GameObject foodPrefab){
		Food f = foodPrefab.GetComponent<Food>();
		string foodName = f.name;
		Debug.Log(f.name);
		if(foodName.Equals("Fried Chicken")){
			if(friedChickenQ < f.total){
				friedChickenQ ++;
				f.quantityTextComp.text = friedChickenQ + "";
			}
				
		}else if(foodName.Equals("Cheese Burger")){
			if(cheeseBurgerQ < f.total){
				cheeseBurgerQ ++;
				f.quantityTextComp.text = cheeseBurgerQ + "";
			}
		}else if(foodName.Equals("Coke")){
			if(cokeQ < f.total){
				cokeQ ++;
				f.quantityTextComp.text = cokeQ + "";
			}
		}
		UpdateTotal();

	}

	public void OnMinusFoodQuantity(GameObject foodPrefab){
		Food f = foodPrefab.GetComponent<Food>();
		string foodName = f.name;
		if(foodName.Equals("Fried Chicken")){
			if(friedChickenQ >= 1){
				friedChickenQ--;
				f.quantityTextComp.text = friedChickenQ + "";
			}
			
		}else if(foodName.Equals("Cheese Burger")){
			if(cheeseBurgerQ >= 1){
				cheeseBurgerQ--;
				f.quantityTextComp.text = cheeseBurgerQ + "";
			}
			
		}else if(foodName.Equals("Coke")){
			if(cokeQ >= 1){
				cokeQ--;
				f.quantityTextComp.text = cokeQ + "";
			}
			
		} 
		UpdateTotal();
	}
}
