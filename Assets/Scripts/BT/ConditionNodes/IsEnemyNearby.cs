using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyNearby : BTNode {

	public IsEnemyNearby(GCS _unit){
		unit = _unit;
	}

	public override int Execute(){
		unit.ClearEnemyList();
		for(int i=0; i < unit.enemies.Count; i++){
			if(unit.enemies[i].shootable){
				return 0;
			}
		}
		return -1;
	}
}
