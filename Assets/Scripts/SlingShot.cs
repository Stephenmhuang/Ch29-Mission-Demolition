using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour {
	static public SlingShot S;

	//fields set in the unity inspector pane
	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool _____;
	//fields set dynammically
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	void Awake(){
		S = this;
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (true);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter() {
	
		//print ("SlingShot:OnMouseEnter()");
		launchPoint.SetActive (true);
	
	}

	void OnMouseExit() {
		//print ("SlingShot:OnMouseExit()");
		launchPoint.SetActive(false);
	}

	void OnMouseDown(){
		//player pressed down mouse button over slingshot
		aimingMode = true;
		//instantiate a projectile
		projectile = Instantiate( prefabProjectile ) as GameObject;
		//starting at launch point
		projectile.transform.position = launchPos;
		//set it to isKinematic for now
		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//if slingshot is not in aiming mode, dont run this code
		if(!aimingMode) return;

		//get the current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;

		//convert the mouse position to 3d coord
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

		//find delta from launchPos to mousePos3D
		Vector3 mouseDelta = mousePos3D-launchPos;

		//limit mouseDelta to the radius of the slingshot SphereCollider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize ();
			mouseDelta *= maxMagnitude;
		}

		//move projectile to new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		if (Input.GetMouseButtonUp (0)) {
			//the mouse has been released
			aimingMode = false;
			projectile.GetComponent<Rigidbody>().isKinematic = false;
			projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
			FollowCam.S.poi = projectile;
			projectile = null;
			MissionDemolition.ShotFired ();
		}
		
	}
}
