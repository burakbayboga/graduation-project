using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUnitInPosition : BTNode {

	public MakeUnitInPosition(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		unit.currentCommand = "InPosition";
		unit.rWTarget = new Vector3(-1f, -1f, -1f);
		unit.gridTarget = new Vector2(-1f, -1f);
		return 0;
	}
}
