using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diffuse : BTNode {

	int emptySlot;
	Vector3 currentPos;
	List<Vector3> availableSpots;
	Vector3 previousTarget;


	public Diffuse(GCS _unit, int _emptySlot){
		unit = _unit;
		emptySlot = _emptySlot;
		previousTarget = new Vector3(-1f, -1f, -1f);
		availableSpots = new List<Vector3>();
	}

	public override int Execute(){

		currentPos = unit.gameObject.transform.position;
		if(currentPos == previousTarget){
			return 0;
		}
		availableSpots = GetAvailableSpots();

		Vector3 targetPos = availableSpots[Random.Range(0, availableSpots.Count)];
		previousTarget = targetPos;
		unit.mover.GetMoving((int)(targetPos.x), (int)(targetPos.y));
		return 1;

	}

	List<Vector3> GetAvailableSpots(){
		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(currentPos, 10f, layerMask);
		GOS currentCover;
		//availableSpots.Clear();
		List<Vector3> _availableSpots = new List<Vector3>();
		for(int i=0; i < coverColliders.Length; i++){
			currentCover = coverColliders[i].gameObject.GetComponent<GOS>();
			for(int j=0; j < 8; j++){
				if(currentCover.possibilities[j] == emptySlot){
					_availableSpots.Add(currentCover.coordinates[j]);
				}
			}
		}
		return _availableSpots;
	}
}