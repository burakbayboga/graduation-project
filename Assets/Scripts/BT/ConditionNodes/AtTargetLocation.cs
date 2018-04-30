using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtTargetLocation : BTNode {

	public AtTargetLocation(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.gameObject.transform.position == unit.rWTarget){
			return 0;
		}
		return -1;
	}

}
