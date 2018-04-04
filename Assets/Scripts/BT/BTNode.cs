using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BTNode {

	public List<BTNode> children;
	public GCS unit;

	public BTNode(){

	}

	public BTNode(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
	}

	public virtual int Execute(){
		return 0;
	}

}
