using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	static public FollowCam S; //followcam singleton
	public float easing = 0.05f;
	public Vector2 minXY;

	//feilds set in the Unity Inspector Pane
	public bool ___________________;

	//feilds set dynamically
	public GameObject poi; //point of interest
	public float camZ; //desired z position of the camera


	void Awake() {
		S = this;
		camZ = this.transform.position.z;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if theres only one line following an if, it doesnt need braces
		if (poi == null) return; //return if no poi

		//get the position of the poi
		Vector3 destination = poi.transform.position;

		//limit the x and y to min values
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);

		//interpolate from the current Camera position toward destination
		destination = Vector3.Lerp(transform.position, destination, easing);

		//retain a destination.z of camZ
		destination.z = camZ;

		//set the camera to the desitnation
		transform.position = destination;

		//set the orthagraphic dize of the camera to keep ground in view
		this.GetComponent<Camera>().orthographicSize = destination.y +10;
	}
}
