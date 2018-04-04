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
		if(unit.currentTarget == previousTarget){
			return 0;
		}
		previousTarget = unit.currentTarget;
		unit.mover.GetMoving((int)(unit.currentTarget.x), (int)(unit.currentTarget.y));
		return 1;
	}
	
}
