using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpotCandidate {

	public Vector3 pos;
	//public int coverLevel;

	//lower cost is higher in priority
	public float cost;
	

	public CoverSpotCandidate(Vector3 _pos){
		pos = _pos;		
	}

	public void ApplyDistanceFactor(Vector3 unitPos, float moveSpeed){
		cost += (unitPos - pos).magnitude * moveSpeed;
	}

	public void ApplyEnemyHitDieFactor(List<Enemy> unitsThatCanShootMe, int AC){
		int layerMask = 1 << 10;
		RaycastHit[] hits;
		Vector3 direction;
		Enemy currentEnemy;
		for(int i=0; i < unitsThatCanShootMe.Count; i++){
			currentEnemy = unitsThatCanShootMe[i];
			direction = currentEnemy.position - pos;
			hits = Physics.RaycastAll(pos, direction, direction.magnitude, layerMask);
			
			bool behindCoverWRTE = false;	//behind cover with respect to current enemy
			int coverLevel = 0;
			for(int j=0; j < hits.Length; j++){
				if((hits[j].transform.position - pos).magnitude < 1.5f){
					behindCoverWRTE = true;
					coverLevel = hits[j].transform.gameObject.GetComponent<GOS>().coverLevel;
					break;
				}
			}
			float hitDieExpectedValue = HitDieExpectedValue(currentEnemy.unitSheet.hitDieType, currentEnemy.unitSheet.hitDieCount, coverLevel, behindCoverWRTE);
			//if(AC < hitDieExpectedValue){
				cost += (hitDieExpectedValue - AC) * 2;
			//}
		}
	}

	float DieExpectedValue(int hitDieType){
		float expectedValue = 0f;
		for(int i=1; i < hitDieType+1; i++){
			expectedValue += ((1/hitDieType) * i);
		}
		return expectedValue;
	}

	float HitDieExpectedValue(int hitDieType, int hitDieCount, int coverLevel, bool behindCoverWRTE){

		//TODO: fix 4 - coverLevel part
		if(behindCoverWRTE){
			return (DieExpectedValue(hitDieType) * hitDieCount) - (hitDieType / (4 - coverLevel));
		}
		else{
			return DieExpectedValue(hitDieType) * hitDieCount;
		}
	}

}
