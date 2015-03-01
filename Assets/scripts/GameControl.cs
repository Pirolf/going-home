using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class GameControl : MonoBehaviour {
	public static GameObject player;
	public static GameObject playerDataCanvas;
	public static GameControl gameControl;
	public static Vector3 locInMain = Vector3.zero; //location in main scene
	public static Quaternion rotInMain =  Quaternion.identity;
	public static Vector3 colliderPos; //collider with houses
	public enum GameState{
		PlayerNavigating,
		HumanInteraction
	};
	public static int gameState = (int)GameState.PlayerNavigating;



	//return to main scene
	public void OnLeaveCurrScene(GameObject leaveCanvas){
		Debug.Log("back to outside");
		
		leaveCanvas.SetActive(false);
		player.GetComponent<PlayerController>().EnableMouseLook(true);
		RecoverLocInMain();
	}
	//only should be called when exit main scene
	public void SaveLocInMain(){
		locInMain = player.transform.position;
		rotInMain = player.transform.rotation;
	}

	//set player position to last saved (loc in main)
	//should only be called when return to main scene from others
	public void RecoverLocInMain(){
		//ray.getpoint
		if(colliderPos == null){
			Debug.Log("Last collision position missing!");
			return;
		}
		Ray colliderAndPlayer = new Ray(colliderPos,colliderPos-locInMain);
		Vector3 p = colliderAndPlayer.GetPoint(1.0f);

		player.transform.position = p;
		player.transform.rotation = rotInMain;
	}
	
	
	void Awake(){
		player = GameObject.Find("homelessPlayer");
		playerDataCanvas = GameObject.Find("PlayerDataCanvas");
		
	}
	void Start () {
		gameControl = this;

	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Load(){
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData pd = (PlayerData)(bf.Deserialize(fs));
			fs.Close();
		}
		
	}
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/playerInfo.dat", (int)FileMode.Open);
		PlayerData pd = PlayerData.playerData;
		pd.health = 50;

		bf.Serialize(fs, pd);
		fs.Close();
	}
}


