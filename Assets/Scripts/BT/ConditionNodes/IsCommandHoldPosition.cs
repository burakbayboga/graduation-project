using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCommandHoldPosition : BTNode {

	public IsCommandHoldPosition(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.currentCommand == "hold_position"){
			return 0;
		}
		return -1;
	}
}
