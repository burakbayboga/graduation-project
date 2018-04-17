using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeCover_Deprecated : BTNode {

	Vector3 currentPos;
	List<int> possibilities;
	Collider[] coverColliders;
	Vector3 previousTarget;
	int emptySlot;

	public TakeCover_Deprecated(GCS _unit, int _emptySlot){
		emptySlot = _emptySlot;
		unit = _unit;
		possibilities = new List<int>();
		for(int i=0; i < 8; i++){
			possibilities.Add(emptySlot);
		}
		previousTarget = new Vector3(-1f, -1f, -1f);
	}

	public override int Execute(){
		return 0;
		/*currentPos = unit.gameObject.transform.position;
		coverColliders = GetColliders();

		bool coverExists = false;
		int coverIndex = -1;
		for(int i=0; i < coverColliders.Length; i++){
			//KILL IT WITH FIRE
			for(int j=0; j < coverColliders[i].gameObject.GetComponent<GOS>().possibilities.Count; j++){
				possibilities[j] = coverColliders[i].gameObject.GetComponent<GOS>().possibilities[j];
			}
			EliminatePossibilitiesByDistance(coverColliders[i].gameObject.GetComponent<GOS>());
			for(int j=0; j < unit.enemies.Count; j++){
				EliminatePossibilitiesBySection(GetEnemySection(unit.enemies[j].position, coverColliders[i].gameObject.transform.position));
			}
			for(int j=0; j < 8; j++){
				if(possibilities[j] == emptySlot || possibilities[j] == unit.gameObject.GetInstanceID()){
					coverExists = true;
					coverIndex = i;
					break;
				}
			}
			if(coverExists){
				break;
			}
		}

		if(!coverExists){
			return -1;
		}

		Vector3 targetPos = GetClosestCoordinate(coverIndex);
		if(targetPos == previousTarget){
			return 0;
		}

		previousTarget = targetPos;
		unit.mover.GetMoving((int)(targetPos.x), (int)(targetPos.y));
		return 1;*/
	}

	

	Vector3 GetClosestCoordinate(int coverIndex){
		float lowestDistance = float.MaxValue;
		float tempDistance = 0;
		int lowestIndex = -1;
		GOS chosenCover = coverColliders[coverIndex].gameObject.GetComponent<GOS>();

		for(int i=0; i < 8; i++){
			if(possibilities[i] != emptySlot && possibilities[i] != unit.gameObject.GetInstanceID()){
				continue;
			}
			tempDistance = (chosenCover.coordinates[i] - unit.gameObject.transform.position).sqrMagnitude;
			if(tempDistance < lowestDistance){
				lowestDistance = tempDistance;
				lowestIndex = i;
			}
		}

		return chosenCover.coordinates[lowestIndex];
	}

	Collider[] GetColliders(){
		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(currentPos, unit.rangeCollider.radius, layerMask);
		Array.Sort(coverColliders, (c1, c2) => ((currentPos - c1.gameObject.transform.position).sqrMagnitude).CompareTo((currentPos - c2.gameObject.transform.position).sqrMagnitude));
		return coverColliders;
	}

	int GetEnemySection(Vector3 enemyPos, Vector3 coverPos){
		Vector3 coverToEnemy = enemyPos - coverPos;
		float angle = Vector3.SignedAngle(coverToEnemy, Vector3.up, Vector3.forward);

		if(angle >= -22.5f && angle <= 22.5f){
			return 0;
		}
		if(angle >= 22.5f && angle <= 67.5f){
			return 1;
		}
		if(angle >= 67.5f && angle <= 112.5f){
			return 2;
		}
		if(angle >= 112.5f && angle <= 157.5f){
			return 3;
		}
		if(angle >= 157.5f || angle <= -157.5f){
			return 4;
		}
		if(angle >= -157.5f && angle <= -112.5f){
			return 5;
		}
		if(angle >= -112.5f && angle <= -67.5f){
			return 6;
		}
		if(angle >= -67.5f && angle <= -22.5f){
			return 7;
		}

		//BLOOD FOR THE BLOOD GOD
		return -1;

	}

	void EliminatePossibilitiesBySection(int section){
		for(int i=section-2; i < section+3; i++){
			if(i < 0){
				possibilities[i+8] = 1;
			}
			else if(i >= 8){
				possibilities[i-8] = 1;
			}
			else{
				possibilities[i] = 1;
			}
		}
	}

	void EliminatePossibilitiesByDistance(GOS cover){
		bool closeEnough;
		for(int i=0; i < cover.coordinates.Count; i++){
			closeEnough = false;
			for(int j=0; j < unit.enemies.Count; j++){
				if((cover.coordinates[i] - unit.enemies[j].position).magnitude <= unit.rangeCollider.radius){
					closeEnough = true;
					break;
				}
			}
			if(!closeEnough){
				possibilities[i] = 1;
			}
		}
	}
}
