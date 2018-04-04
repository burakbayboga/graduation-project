using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyWeakerThanUs : BTNode {

	public IsEnemyWeakerThanUs(GCS _unit){
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
			weThePeople += allies[i].GetComponent<GCS>().hitDieType * allies[i].GetComponent<GCS>().hitDieCount;
		}

		int theyThePeople = 0;
		for(int i=0; i < enemies.Length; i++){
			theyThePeople += enemies[i].GetComponent<GCS>().hitDieType * enemies[i].GetComponent<GCS>().hitDieCount;
		}
		
		if(theyThePeople <= weThePeople){
			return 0;
		}
		return -1;
	}
	
}
