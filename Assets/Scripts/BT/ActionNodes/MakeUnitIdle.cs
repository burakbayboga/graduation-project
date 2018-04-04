using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUnitIdle : BTNode {

	public MakeUnitIdle(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		unit.currentCommand = "idle";
		return 0;
	}
	
}
