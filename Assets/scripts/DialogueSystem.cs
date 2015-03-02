using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour {
	public static DialogueSystem dialogueSystem;
	public int begItem;
	public int subactionChoice;

	public enum BegItem{
		Money,
	};
	public GameObject diaglogueText;
	public GameObject diaglogueCanvas;
	public GameObject actionPanel;
	public GameObject subactionPanel;
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
		SubActionBeg,
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
		ResetDialogueSys();

		diaglogueTextComp = diaglogueText.GetComponent<Text>();
		decisonQuantityTextComp = decisonQuantityText.GetComponent<Text>();

	}
	void Start () {
	
	}
	public void ShowMainActions(){
		actionPanel.SetActive(true);
	}
	public void ShowSubactions(){
		actionPanel.SetActive(false);
		subactionPanel.SetActive(true);

	}
	public void StartInteraction(float NPCtemper, bool NPCbeggedToday){
		//enable diaglogue canvas
		//set init text
		dialogueState = (int)DialogueState.Greeting;
		diaglogueCanvas.SetActive(true);
		StartCoroutine(Interaction(NPCtemper, NPCbeggedToday));
	}
	public void ShowDecision(int amount){
		if(begItem == (int)BegItem.Money){
			//update raw image of money and quantity
			//decisonRawImage.GetComponent<RawImage>().texture = ;
			decisonQuantityTextComp.text = amount + "";
			subactionPanel.SetActive(false);
			decisionPanel.SetActive(true);
			//update player money
			PlayerData.playerData.money += amount;
			PlayerData.playerData.UpdateStatsDisplay();
		}
	}
	public int MakeDecision(float NPCtemper){
		float r = UnityEngine.Random.Range(0,10)/10.0f;
		int money = 0;
		if(NPCtemper > 0.4){
			if(subactionChoice == (int)SubactionChoice.Bad){
				//diaglogueTextComp.text = "";
				if(r > 0.8){
					money = 1;
				}
			}else if(subactionChoice == (int)SubactionChoice.Neutral){
				//diaglogueTextComp.text = "";
				if(r > 0.5){
					money = 1;
				}
			}else if(subactionChoice == (int)SubactionChoice.Good){
				//diaglogueTextComp.text = "";
				if(r > 0.3){
					money = Mathf.FloorToInt(UnityEngine.Random.Range(1,3));
					Debug.Log("good" + money);
				}
			}
		}else{
			//doesn't give anything
			money = 0;
		}
		return money;

	}
	IEnumerator Interaction(float NPCtemper, bool beggedToday){
		
		while(GameControl.gameState == (int)GameControl.GameState.HumanInteraction){
			//disable mouselook
			GameControl.player.GetComponent<PlayerController>().disableMove = true;
			GameControl.player.GetComponent<PlayerController>().EnableMouseLook(false);
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
				yield return new WaitForSeconds(0.5f);
				begItem = (int)BegItem.Money;
				if(NPCtemper < 0.6){
					//bad temper
					diaglogueTextComp.text = "urhhh...god...why?";
				}else{
					//neutral
					diaglogueTextComp.text = "I'm sorry, but may I ask why?";	
				}
				dialogueState = (int)DialogueState.SubActionBeg;
			}
			else if(dialogueState == (int)DialogueState.SubActionBeg){
				//if player chose badguy 0, 1, 2
				yield return new WaitForSeconds(0.5f);
				ShowSubactions();
				//give money
				
				//dialogueState = (int)DialogueState.Decision;

			}
			else if(dialogueState == (int)DialogueState.Decision){
				yield return new WaitForSeconds(1f/30f);
				//TODO: make decison

				int money = MakeDecision(NPCtemper);
				//show text
				if(money > 0){
					diaglogueTextComp.text = "Alright, here you go man!";
				}else{
					diaglogueTextComp.text = "Sorry I don't have change on me.";
				}
				yield return new WaitForSeconds(1f/3f);
				ShowDecision(money);
				yield return new WaitForSeconds(3f);
				dialogueState = (int)DialogueState.EndDialogue;
			}
			else if(dialogueState == (int)DialogueState.EndDialogue){
				ResetDialogueSys();
				GameControl.gameState = (int)GameControl.GameState.PlayerNavigating;
				diaglogueCanvas.SetActive(false);
				GameControl.player.GetComponent<PlayerController>().EnableMouseLook(true);
				GameControl.player.GetComponent<PlayerController>().disableMove = false;
			}

		}//end while

	}
	public void OnClick_BegForMoney(){
		dialogueState = (int)DialogueState.MainActionBegMoney;
	}
	public void OnClick_Nevermind(){
		dialogueState = (int)DialogueState.MainActionCancelDialogue;
	}

	public void OnClick_Subaction(int choice){
		subactionChoice = choice;
		dialogueState = (int)DialogueState.Decision;
	}
	//called when interaction is over
	public void ResetDialogueSys(){
		diaglogueCanvas.SetActive(false);
		actionPanel.SetActive(false);
		subactionPanel.SetActive(false);
		decisionPanel.SetActive(false);
		dialogueState = (int)DialogueState.Greeting;
	}
	// Update is called once per frame
	void Update () {
	
	}

}
