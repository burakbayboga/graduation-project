using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpotsCollider : MonoBehaviour {

	GOS cover;
	public int spot;
	public int emptySlot;

	void Start(){
		cover = transform.parent.gameObject.GetComponent<GOS>();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "unit"){
			cover.possibilities[spot] = other.gameObject.GetInstanceID();
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "unit"){
			cover.possibilities[spot] = emptySlot;
		}
	}
}
