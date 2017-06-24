using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
	static public bool goalMet = false;
	Renderer rend;
	void OnTriggerEnter ( Collider other) {
		if (other.gameObject.tag == "Projectile"){
			Goal.goalMet = true;
			rend = GetComponent<Renderer> ();
			Color c = rend.material.color;
			c.a = 1;
			rend.material.color = c;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
