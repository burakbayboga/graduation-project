using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overwhelmed : BTNode {

	public Overwhelmed(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		int layerMask = 1 << unit.gameObject.layer;
		Collider[] allies = Physics.OverlapSphere(unit.gameObject.transform.position, 15f, layerMask);
		if(unit.gameObject.layer == 8){
			layerMask = 1 << 9;
		}
		else{
			layerMask = 1 << 8;
		}
		Collider[] enemies = Physics.OverlapSphere(unit.gameObject.transform.position, 15f, layerMask);
		
		int weThePeople = 0;
		for(int i=0; i < allies.Length; i++){
			weThePeople += allies[i].GetComponent<GCS>().personCount;
		}
		int theyThePeople = 0;
		for(int i=0; i < enemies.Length; i++){
			theyThePeople += enemies[i].GetComponent<GCS>().personCount;
		}
		if(theyThePeople > weThePeople*2){
			return 0;
		}
		return -1;
	}

}
