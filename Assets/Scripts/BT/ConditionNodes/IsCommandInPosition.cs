using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCommandInPosition : BTNode {

	public IsCommandInPosition(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.currentCommand == "InPosition"){
			return 0;
		}
		return -1;
	}
	
}
