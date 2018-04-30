using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSF0 : BTNode {

	float threshold;

	public CSF0(GCS _unit, float _threshold){
		children = new List<BTNode>();
		unit = _unit;
		threshold = _threshold;
	}

	public override int Execute(){
		if(unit.currentStress >= threshold){
			return children[0].Execute();
		}
		else{
			return children[1].Execute();
		}
	}
}
