using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSF0 : BTNode {

	public CSF0(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
	}

	public override int Execute(){
		if(unit.currentStress >= 50){
			return children[0].Execute();
		}
		else{
			return children[1].Execute();
		}
	}
}
