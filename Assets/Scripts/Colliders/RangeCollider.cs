using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour {

	GCS unit;

	void Start(){
		unit = transform.parent.gameObject.GetComponent<GCS>();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "unit" && other.gameObject.GetComponent<GCS>().client != unit.client){
			int index = unit.IndexOfEnemy(other.gameObject);
			if(index == -1){
				Enemy newEnemy = new Enemy(other.gameObject);
				newEnemy.shootable = true;
				unit.enemies.Add(newEnemy);
			}
			else{
				unit.enemies[index].shootable = true;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "unit" && other.gameObject.GetComponent<GCS>().client != unit.client){
			unit.enemies[unit.IndexOfEnemy(other.gameObject)].shootable = false;
		}
	}
}
