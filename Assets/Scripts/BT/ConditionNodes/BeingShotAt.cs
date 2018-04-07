using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingShotAt : BTNode {

	public BeingShotAt(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
	}

	public override int Execute(){
		unit.ClearEnemyList();
		for(int i=0; i < unit.enemies.Count; i++){
			if(unit.enemies[i].canShootMe){
				return 0;
			}
		}
		return -1;
	}
}
