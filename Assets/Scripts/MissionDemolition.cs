using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode{
	idle,
	playing,
	levelEnd
}

public class MissionDemolition : MonoBehaviour {
	static public MissionDemolition S;
	 

	//feilds set in unity inspector pane
	public GameObject[] castles;
	public GUIText gtLevel;
	public GUIText gtScore;
	public Vector3 castlePos;

	public bool __________________;

	//fields set dynamically
	public int level;
	public int levelMax;
	public int shotsTaken;
	public GameObject castle;
	public GameMode mode = GameMode.idle;
	public string showing = "Slingshot";

	// Use this for initialization
	void Start () {
		S = this;
		level = 0;
		levelMax = castles.Length;
		StartLevel ();
	}

	void StartLevel(){
		//get rid of old castle if exists
		if (castle != null) {
			Destroy (castle);
		}

		//destroy old projectiles if exists
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
		foreach (GameObject pTemp in gos){
			Destroy (pTemp);
		}

		//instantiate new castle
		castle = Instantiate(castles[level]) as GameObject;
		castle.transform.position = castlePos;
		shotsTaken = 0;

		//reset the camera
		SwitchView("Both");
		ProjectileLine.S.Clear ();

		//reset the goal
		Goal.goalMet = false;

		ShowGT ();
		mode = GameMode.playing;
	}

	void ShowGT(){
		//show level data in GUI
		gtLevel.text = "Level: "+(level+1)+" of "+levelMax;
		gtScore.text = "Shots Taken: " + shotsTaken;
	}
	
	// Update is called once per frame
	void Update () {
		ShowGT ();

		//check for level end
		if (mode == GameMode.playing && Goal.goalMet) {
			//change mode to stop checking for level end
			mode = GameMode.levelEnd;
			//zoom out
			SwitchView ("Both");
			//start the new level in 2 seconds
			Invoke ("NextLevel", 2f);
		}
		
	}


	void NextLevel(){
		level++;
		if (level == levelMax) {
			level = 0;
		}
		StartLevel ();
	}

	void OnGUI(){
		//draw the gui button for view switching at top of screen
		Rect buttonRect = new Rect ((Screen.width / 2) - 50, 10, 100, 24);

		switch (showing) {
		case "Slingshot":
			if (GUI.Button (buttonRect, "Show Castle")) {
				SwitchView ("Castle");
			}
			break;
		case "Castle":
			if (GUI.Button (buttonRect, "Show Both")) {
				SwitchView ("Both");
			}
			break;
		case "Both":
			if (GUI.Button (buttonRect, "Show Slingshot")) {
				SwitchView ("Slingshot");
			}
			break;
		}
	}

	//static method that allows code anywhere to requeist a view change
	static public void SwitchView(string eView){
		S.showing = eView;

		switch (S.showing) {
		case "Slingshot":
			FollowCam.S.poi = null;
			break;

		case "Castle":
			FollowCam.S.poi = S.castle;
			break;

		case "Both":
			FollowCam.S.poi = GameObject.Find ("ViewBoth");
			break;
		}
	}

	//static method that sllows code anywhere to increment shotsTaken
	public static void ShotFired(){
		S.shotsTaken++;
	}
}
