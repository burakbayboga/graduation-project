using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode : BTNode {

	public InverterNode(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
	}

	public override int Execute(){
		int childStasus = children[0].Execute();
		if(childStasus == 0){
			return -1;
		}
		else{
			return 0;
		}
	}
	
}
