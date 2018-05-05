using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BTNode {

	//Vector3 currentPos;
	List<Enemy> unitsThatCanShootMe;
	Vector3 firstChargePos;
	string stressResolution;

	public Charge(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
		unitsThatCanShootMe = new List<Enemy>();
		firstChargePos = new Vector3(-1f, -1f, -1f);
		stressResolution = "charging";
	}

	public override int Execute(){
		if(unit.transform.position == firstChargePos){
			firstChargePos = new Vector3(-1f, -1f, -1f);
			unit.charging = false;
			return 0;
		}
		if(unit.charging && !unit.chargeFail){
			return 1;
		}
		if(unit.charging && unit.chargeFail){
			unit.chargeFail = false;
			firstChargePos = new Vector3(-1f, -1f, -1f);
		}

		//currentPos = unit.gameObject.transform.position;
		unit.ClearEnemyList();
		unit.UpdateEnemyPositions();

		for(int i=0; i < unit.enemies.Count; i++){
			if(unit.enemies[i].canShootMe){
				unitsThatCanShootMe.Add(unit.enemies[i]);
			}
		}

		Enemy targetEnemy = unit.enemies[Random.Range(0,unitsThatCanShootMe.Count)];
		Vector2 target = GRWInterface.GetRWToGrid((int)(targetEnemy.position.x), (int)(targetEnemy.position.y));
		if(firstChargePos != new Vector3(-1f, -1f, -1f)){
			return 1;
		}

		firstChargePos = target;
		unit.mover.GetMovingRW(target);
		unit.charging = true;
		return 1;
	}

}
