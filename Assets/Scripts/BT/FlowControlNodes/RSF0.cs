using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSF0 : BTNode {

	public RSF0(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
	}

	public override int Execute(){
		//if(unit.stressResolution == "calm"){
			if(unit.currentStress >= 50){
				return children[Random.Range(0,2)].Execute();
				//unit.stressResolution = "charging";
			}
			else{
				return children[0].Execute();
			}
		//}
		//else{
			//return children[1].Execute();
		//}
	}

}
