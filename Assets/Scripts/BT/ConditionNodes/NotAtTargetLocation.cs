using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAtTargetLocation : BTNode {

	public NotAtTargetLocation(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.gameObject.transform.position != unit.currentTarget){
			return 0;
		}
		return -1;
	}
}
