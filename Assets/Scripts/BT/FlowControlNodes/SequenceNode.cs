using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : BTNode {

	public SequenceNode(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
		
	}

	public override int Execute(){
		for(int i=0; i < children.Count; i++){
			int childStasus = children[i].Execute();
			if(childStasus == -1){
				return -1;
			}
			else if(childStasus == 1){
				return 1;
			}
		}
		return 0;
	}
}
