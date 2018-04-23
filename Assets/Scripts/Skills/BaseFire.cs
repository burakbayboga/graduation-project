using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFire : MonoBehaviour {

	public virtual void Fire(GameObject _shooter){

	}

	public int EnemyCoverLevel(Vector3 enemyPos, Vector3 shooterPos){
		int layerMask = 1 << 10;
		RaycastHit[] hits;
		Vector3 shootDirection = enemyPos - shooterPos;
		hits = Physics.RaycastAll(shooterPos, shootDirection, shootDirection.magnitude, layerMask);
		for(int i=0; i < hits.Length; i++){
			if((hits[i].transform.position - enemyPos).magnitude < 1.5f){
				return hits[i].transform.gameObject.GetComponent<GOS>().coverLevel;
			}
		}
		return 0;
	}

	public void AlertEnemiesByGunNoise(GameObject shooter, Vector3 noiseOrigin){
		
		GCS shooterSheet = shooter.GetComponent<GCS>();
		int layerMask;
		if(shooter.layer == 8){
			layerMask = 1 << 9;
		}
		else{
			layerMask = 1 << 8;
		}
		Collider[] alertedUnits = Physics.OverlapSphere(noiseOrigin, shooterSheet.gunNoiseReach, layerMask);
		for(int i=0; i < alertedUnits.Length; i++){
			alertedUnits[i].gameObject.GetComponent<GCS>().AlertByGunNoise(shooter);
		}
	}

	

	
}
