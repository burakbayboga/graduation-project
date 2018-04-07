using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this version takes priorities into account
//finds best possible cover instead of THE perfect cover
//the lower the spot value, the better it is
//negative means not vacant
public class TakeCover : BTNode {

	Vector3 currentPos;
	List<int> possibilities;
	Collider[] coverColliders;
	Vector3 previousTarget;
	int emptySlot;
	List<CoverSpotCandidate> candidates;

	public TakeCover(GCS _unit, int _emptySlot){
		emptySlot = _emptySlot;
		unit = _unit;
		/*possibilities = new List<int>();
		for(int i=0; i < 8; i++){
			possibilities.Add(emptySlot);
		}*/
		previousTarget = new Vector3(-1f, -1f, -1f);
		candidates = new List<CoverSpotCandidate>();
	}

	public override int Execute(){
		currentPos = unit.gameObject.transform.position;
		coverColliders = GetColliders();
		GetCandidates();
		/*for(int i=0; i < candidates.Count; i++){
			Debug.Log(candidates[i].pos);
		}
		return 0;*/

		//no possible cover
		if(candidates.Count == 0){
			return -1;
		}

		for(int i=0; i < candidates.Count; i++){
			candidates[i].ApplyDistanceFactor(currentPos, unit.moveSpeed);
		}
		unit.ClearEnemyList();
		UpdateEnemyPositions();
		List<Enemy> unitsThatCanShootMe = new List<Enemy>();
		unitsThatCanShootMe = GetUnitsThatCanShootMe();
		for(int i=0; i < candidates.Count; i++){
			candidates[i].ApplyEnemyHitDieFactor(unitsThatCanShootMe, unit.armorClass);
		}

		//sort candidates for chaotic decision making
		//SortCandidatesByCost();
		Vector3 targetPos = GetLowestCostCandidate();
		//Debug.Log(targetPos);
		if(targetPos == previousTarget){
			return 0;
		}
		previousTarget = targetPos;
		unit.mover.GetMoving((int)(targetPos.x), (int)(targetPos.y));
		return 1;


	}

	Vector3 GetLowestCostCandidate(){
		float lowestCost = float.MaxValue;
		int lowestIndex = -1;
		for(int i=0; i < candidates.Count; i++){
			if(candidates[i].cost < lowestCost){
				lowestCost = candidates[i].cost;
				lowestIndex = i;
			}
		}
		return candidates[lowestIndex].pos;
	}

	void SortCandidatesByCost(){
		candidates.Sort((c1, c2) => c1.cost.CompareTo(c2.cost));
	}

	void UpdateEnemyPositions(){
		for(int i=0; i < unit.enemies.Count; i++){
			unit.enemies[i].position = unit.enemies[i].unit.transform.position;
		}
	}

	List<Enemy> GetUnitsThatCanShootMe(){
		List<Enemy> tempUnitsThatCanShootMe = new List<Enemy>();
		for(int i=0; i < unit.enemies.Count; i++){
			if(unit.enemies[i].canShootMe){
				tempUnitsThatCanShootMe.Add(unit.enemies[i]);
			}
		}
		return tempUnitsThatCanShootMe;
	}


	Collider[] GetColliders(){
		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(currentPos, unit.rangeCollider.radius, layerMask);
		//Array.Sort(coverColliders, (c1, c2) => ((currentPos - c1.gameObject.transform.position).sqrMagnitude).CompareTo((currentPos - c2.gameObject.transform.position).sqrMagnitude));
		return coverColliders;
	}

	
	void GetCandidates(){
		candidates.Clear();
		for(int i=0; i < coverColliders.Length; i++){
			GOS currentCover = coverColliders[i].gameObject.GetComponent<GOS>();
			for(int j=0; j < 8; j++){
				if(currentCover.possibilities[j] == emptySlot || currentCover.possibilities[j] == unit.gameObject.GetInstanceID()){
					CoverSpotCandidate newCandidate = new CoverSpotCandidate(currentCover.coordinates[j]);
					candidates.Add(newCandidate);
				}
			}
		}
	}

	

	
}
