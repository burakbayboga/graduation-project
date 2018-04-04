using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToTheHills : BTNode {

	Vector3 currentPos;
	Vector3 previousTarget;
	
	public RunToTheHills(GCS _unit){
		children = new List<BTNode>();
		unit = _unit;
		previousTarget = new Vector3(-1f, -1f, -1f);
	}

	public override int Execute(){
		//return 0;
		currentPos = unit.gameObject.transform.position;
		//Debug.Log("run to the hills");
		Vector3 sumOfVectors = Vector3.zero;
		/*for(int i=0; i < unit.unitsThatCanShootThis.Count; i++){
			sumOfVectors += unit.unitsThatCanShootThis[i].transform.position;
			//Debug.Log(unit.unitsThatCanShootThis[i].transform.position);
		}*/
		int enemiesShootingMeCount = 0;
		for(int i=0; i < unit.enemies.Count; i++){
			if(unit.enemies[i].canShootMe){
				sumOfVectors += unit.enemies[i].position;
				enemiesShootingMeCount++;
			}
		}
		Vector3 centerPoint = sumOfVectors / enemiesShootingMeCount;
		//Debug.Log(unit.side + " centerPoint: " + centerPoint);
		
		Vector3 targetPos = new Vector3();
		Vector3 direction = -1 * (centerPoint - currentPos);
		//Debug.Log(unit.side + " direction: " + direction);
		if(direction.x < 0f){
			targetPos.x = currentPos.x - 1f;
		}
		else{
			targetPos.x = currentPos.x + 1f;
		}
		if(direction.y < 0f){
			targetPos.y = currentPos.y - 1f;
		}
		else{
			targetPos.y = currentPos.y + 1f;
		}
		if(targetPos == previousTarget){
			return 0;
		}
		previousTarget = targetPos;
		//Debug.Log(unit.side + " targetPos: " + targetPos);
		//unit.mover.Override();
		//unit.mover.AttackMove((int)(targetPos.x), (int)(targetPos.y));
		unit.mover.GetMoving((int)(targetPos.x), (int)(targetPos.y));
		return 0;
	}
}
