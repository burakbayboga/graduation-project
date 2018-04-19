using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diffuse : BTNode {

	int emptySlot;
	Vector3 currentPos;
	Collider[] coverColliders;
	List<CoverSpotCandidate> candidates;
	Vector3 firstRandomPos;


	public Diffuse(GCS _unit, int _emptySlot){
		unit = _unit;
		emptySlot = _emptySlot;
		candidates = new List<CoverSpotCandidate>();
		firstRandomPos = new Vector3(-1f, -1f, -1f);
	}

	public override int Execute(){

		if(unit.gameObject.transform.position == firstRandomPos){
			firstRandomPos = new Vector3(-1f, -1f, -1f);
			return 0;
		}
		if(unit.diffusing && !unit.diffusionFail){
			return 1;
		}
		if(unit.diffusing && unit.diffusionFail){
			firstRandomPos = new Vector3(-1f, -1f, -1f);
		}
		
		currentPos = unit.gameObject.transform.position;
		coverColliders = GetColliders();
		GetCandidates();

		if(candidates.Count == 0){
			//FIX FIX FIX
			//no cover to hold position case
		}
		Vector3 targetPos = candidates[Random.Range(0, candidates.Count)].pos;
		if(firstRandomPos != new Vector3(-1f, -1f, -1f)){
			return 1;
		}
		firstRandomPos = targetPos;
		//unit.sideTracked = true;
		unit.mover.GetMovingRW(new Vector2(targetPos.x, targetPos.y));
		unit.diffusing = true;
		return 1;

	}

	Collider[] GetColliders(){
		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(currentPos, unit.rangeCollider.radius, layerMask);
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