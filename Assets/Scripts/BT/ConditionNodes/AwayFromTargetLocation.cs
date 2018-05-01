using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayFromTargetLocation : BTNode {

	public AwayFromTargetLocation(GCS _unit){
		unit = _unit;
	}
	
	public override int Execute(){
		if(unit.diffusing){
			return -1;
		}
		if((unit.rWTarget - unit.gameObject.transform.position).sqrMagnitude >= 25f){
			return 0;
		}
		return -1;
	}
}