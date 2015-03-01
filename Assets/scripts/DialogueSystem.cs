using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour {
	public static DialogueSystem dialogueSystem;
	public int begItem;
	public int subactionChoice;

	public enum BegItem{
		Money,
		Food,
	};
	public GameObject diaglogueText;
	public GameObject diaglogueCanvas;
	public GameObject actionPanel;
	public GameObject subactionPanelBadGuy;
	public GameObject subactionPanelNeutralGuy;
	public GameObject decisionPanel;
	public GameObject decisonRawImage;
	public GameObject decisonQuantityText;

	private Text decisonQuantityTextComp;

	private Text diaglogueTextComp;

	public string[] greetings = {"hello", "what's up"};
		//
		//player choose interaction
		//	0: sorry(end conversation)
		//	1: beg for money
		//	2: beg for food
		//	3: rob

		//NPC response
		//	0: fuck off (end)
		//	1 or 2: if NPC has bad temper,rand one of below
		//		(0)fuck off (end)
		//		(1)some curse, but will still give money or food (goto 1abc)
		//		(2)some curse,gives a little money/food (end conversation)
		// 	1 or 2: if NPC is netural
		//		(0)i am busy(end conversation)
		//		(1)why should i give you (goto 1def)
		//  3: if NPC is not brave
		//		(0)gives money (end)
		//	3: if NPC is brave
		//		(1)call cops, send player to prison (end)

		//player choose subinteraction
		//  1/2: beg for money/food
		//		(a)please...
		//		(b)explain
		//		(c)who the hell think you are
		//
		//		(d)tell truth
		//		(e)exaggerate
		//		(f)please...beg
	public enum DialogueState{
		Greeting,
		MainActions,
		MainActionCancelDialogue,
		MainActionBegMoney,
		MainActionBegFood,
		SubActionBegBadNPC,
		SubActionBegNeutralNPC,
		Decision,
		EndDialogue,
		Test
	};
	public enum SubactionChoice{
		Bad = 0,
		Neutral = 1,
		Good = 2,
	};
	public int dialogueState;
	// Use this for initialization
	void Awake(){
		dialogueSystem = this;
		diaglogueCanvas.SetActive(false);
		actionPanel.SetActive(false);
		subactionPanelNeutralGuy.SetActive(false);
		subactionPanelBadGuy.SetActive(false);
		decisionPanel.SetActive(false);

		diaglogueTextComp = diaglogueText.GetComponent<Text>();
		decisonQuantityTextComp = decisonQuantityText.GetComponent<Text>();

	}
	void Start () {
	
	}
	public void ShowMainActions(){
		actionPanel.SetActive(true);
	}
	public void ShowSubactionsBadGuy(){
		actionPanel.SetActive(false);
		subactionPanelBadGuy.SetActive(true);

	}
	public void ShowSubactionsNeutralGuy(){
		actionPanel.SetActive(false);
		subactionPanelNeutralGuy.SetActive(true);
	}
	public void StartInteraction(float NPCtemper, float NPCcourage){
		//enable diaglogue canvas
		//set init text
		dialogueState = (int)DialogueState.Greeting;
		diaglogueCanvas.SetActive(true);
		StartCoroutine(Interaction(NPCtemper, NPCcourage));
	}
	public void ShowDecision(int amount){
		if(begItem == (int)BegItem.Money){
			//update raw image of money and quantity
			//decisonRawImage.GetComponent<RawImage>().texture = ;
			decisonQuantityTextComp.text = amount + "";
			subactionPanelBadGuy.SetActive(false);
			subactionPanelNeutralGuy.SetActive(false);
			decisionPanel.SetActive(true);
			//update player money
			PlayerData.playerData.money += amount;
			PlayerData.playerData.UpdateStatsDisplay();
		}
	}
	public void MakeDecision(float NPCtemper){
		float r = UnityEngine.Random.Range(0,1);
		int money = 0;
		if(begItem == (int)BegItem.Money){
			if(NPCtemper > 0.6){
				if(subactionChoice == (int)SubactionChoice.Bad){
					if(r > 0.9){
						money = 1;
					}
				}else if(subactionChoice == (int)SubactionChoice.Neutral){
					if(r > 0.6){
						money = 1;
					}
				}else if(subactionChoice == (int)SubactionChoice.Good){
					if(r > 0.4){
						money = Mathf.FloorToInt(UnityEngine.Random.Range(1,3));
					}
				}
			}else{
				//doesn't give anything
				money = 0;
			}
			ShowDecision(money);

		}else if(begItem == (int)BegItem.Food){
			if(NPCtemper > 0.4){
				if(subactionChoice == (int)SubactionChoice.Bad){

				}else if(subactionChoice == (int)SubactionChoice.Neutral){

				}else if(subactionChoice == (int)SubactionChoice.Good){

				}
			}else{
				//doesn't give anything
			}
		}
	}
	IEnumerator Interaction(float NPCtemper, float NPCcourage){
		
		while(GameControl.gameState == (int)GameControl.GameState.HumanInteraction){
			if(dialogueState == (int)DialogueState.Greeting){
				diaglogueTextComp.text = greetings[0];
				yield return new WaitForSeconds(0.5f);
				//show main actions
				ShowMainActions();
				dialogueState = (int)DialogueState.MainActions;
				//dialogueState = (int)DialogueState.Test;
			}
			else if(dialogueState == (int)DialogueState.Test){
				dialogueState = (int)DialogueState.EndDialogue;
			}
			//current dialogue panel shows main action options to player
			else if(dialogueState == (int)DialogueState.MainActions){
				yield return new WaitForSeconds(1f/30f);
			}
			else if(dialogueState == (int)DialogueState.MainActionCancelDialogue){
				yield return new WaitForSeconds(1f/30f);
				dialogueState = (int)DialogueState.EndDialogue;
			}
			else if(dialogueState == (int)DialogueState.MainActionBegMoney){
				yield return new WaitForSeconds(1f);
				begItem = (int)BegItem.Money;
				if(NPCtemper < 0.6){
					//bad temper
					dialogueState = (int)DialogueState.SubActionBegBadNPC;
				}else{
					//neutral
					dialogueState = (int)DialogueState.SubActionBegNeutralNPC;
				}
			}
			else if(dialogueState == (int)DialogueState.MainActionBegFood){
				yield return new WaitForSeconds(1f);
				begItem = (int)BegItem.Food;
				if(NPCtemper < 0.6){
					//bad temper
					dialogueState = (int)DialogueState.SubActionBegBadNPC;
				}else{
					//neutral
					dialogueState = (int)DialogueState.SubActionBegNeutralNPC;
				}
			}
			else if(dialogueState == (int)DialogueState.SubActionBegBadNPC){
				//if player chose badguy 0, 1, 2
				yield return new WaitForSeconds(1f);
				ShowSubactionsBadGuy();
				//give money or food
				
				//dialogueState = (int)DialogueState.Decision;

			}else if(dialogueState == (int)DialogueState.SubActionBegNeutralNPC){
				//if player chose neutralguy 0,1,2
				yield return new WaitForSeconds(1f);
				ShowSubactionsNeutralGuy();
				//give money or food
				
				//dialogueState = (int)DialogueState.Decision;
			}
			else if(dialogueState == (int)DialogueState.Decision){
				yield return new WaitForSeconds(1f/30f);
				//TODO: make decison

				MakeDecision(NPCtemper);
				yield return new WaitForSeconds(2f);
				dialogueState = (int)DialogueState.EndDialogue;
			}
			else if(dialogueState == (int)DialogueState.EndDialogue){
				GameControl.gameState = (int)GameControl.GameState.PlayerNavigating;
				diaglogueCanvas.SetActive(false);
			}

		}//end while

	}
	public void OnClick_BegForMoney(){
		dialogueState = (int)DialogueState.MainActionBegMoney;
	}
	public void OnClick_BegForFood(){
		dialogueState = (int)DialogueState.MainActionBegFood;
	}
	public void OnClick_Nevermind(){
		dialogueState = (int)DialogueState.MainActionCancelDialogue;
	}

	public void OnClick_SubactionBadGuy(int choice){
		subactionChoice = choice;
		dialogueState = (int)DialogueState.Decision;
	}
	public void OnClick_SubactionNeutralGuy(int choice){
		subactionChoice = choice;
		dialogueState = (int)DialogueState.Decision;
	}
	//called when interaction is over
	public void ResetDialogueSys(){

	}
	// Update is called once per frame
	void Update () {
	
	}

}
