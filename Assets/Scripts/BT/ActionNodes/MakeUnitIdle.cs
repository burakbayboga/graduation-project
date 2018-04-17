using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUnitIdle : BTNode {

	public MakeUnitIdle(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		Debug.Log("makeUnitIdle");
		unit.currentCommand = "idle";
		unit.rWTarget = new Vector3(-1f, -1f, -1f);
		unit.gridTarget = new Vector2(-1f, -1f);
		return 0;
	}
	
}
