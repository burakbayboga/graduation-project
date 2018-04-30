using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overwhelmed : BTNode {

	public Overwhelmed(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		return 0;
	}

}
