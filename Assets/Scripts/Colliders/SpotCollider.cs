using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCollider : MonoBehaviour {

	GCS unit;

	void Start(){
		unit = transform.parent.gameObject.GetComponent<GCS>();
		unit.commandHub.spotters.Add(unit.gameObject);
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "unit" && other.gameObject.GetComponent<GCS>().client != unit.client){
			unit.spottedUnits.Add(other.gameObject);
			GCS traverse;
			for(int i=0; i < unit.commandHub.deployedUnits.Count; i++){
				traverse = unit.commandHub.deployedUnits[i].GetComponent<GCS>();
				if(traverse.IndexOfEnemy(other.gameObject) == -1){
					Enemy newEnemy = new Enemy(other.gameObject);
					traverse.enemies.Add(newEnemy);
				}
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "unit" && other.gameObject.GetComponent<GCS>().client != unit.client){
			unit.spottedUnits.Remove(other.gameObject);
		}
	}
	
}
