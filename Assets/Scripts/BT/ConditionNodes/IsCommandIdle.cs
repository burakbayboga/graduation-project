using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCommandIdle : BTNode {

	public IsCommandIdle(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.currentCommand == "idle"){
			return 0;
		}
		return -1;
	}
	
}
