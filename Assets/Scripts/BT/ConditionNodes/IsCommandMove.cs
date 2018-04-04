using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCommandMove : BTNode {

	public IsCommandMove(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		if(unit.currentCommand == "move"){
			return 0;
		}
		return -1;
	}
}
