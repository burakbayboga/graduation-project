using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTowardsTargetLocation : BTNode {

	Vector3 previousTarget;

	public GoTowardsTargetLocation(GCS _unit){
		unit = _unit;
		previousTarget = new Vector3(-1f, -1f, -1f);
	}

	public override int Execute(){
		//first execute
		if(unit.rWTarget.x == -1f || unit.sideTracked){
			unit.mover.GetMovingGrid((int)(unit.gridTarget.x), (int)(unit.gridTarget.y));
			unit.sideTracked = false;
			return 1;
		}
		return 0;

	}
	
}
