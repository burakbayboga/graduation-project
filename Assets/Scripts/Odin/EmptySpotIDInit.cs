using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpotIDInit : MonoBehaviour {

	int myID;

	void Start(){
		myID = gameObject.GetInstanceID();
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		for(int i=0; i < obstacles.Length; i++){
			obstacles[i].GetComponent<GOS>().emptySlot = myID;
			obstacles[i].GetComponent<GOS>().AssignPossibilityValues();
			CoverSpotsCollider[] coverSpots = obstacles[i].GetComponentsInChildren<CoverSpotsCollider>();
			for(int j=0; j < coverSpots.Length; j++){
				coverSpots[j].emptySlot = myID;
			}
		}
	}
}
