using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShot : BaseFire {

	GameObject shooter;
	Vector3 shooterPos;
	GCS shooterSheet;

	public override void Fire(GameObject _shooter){
		shooter = _shooter;
		shooterSheet = shooter.GetComponent<GCS>();
		shooterPos = shooter.transform.position;

		shooterSheet.ClearEnemyList();
		if(shooterSheet.enemies.Count == 0){
			return;
		}

		for(int i=0; i < shooterSheet.enemies.Count; i++){
			shooterSheet.enemies[i].position = shooterSheet.enemies[i].unit.transform.position;
		}
		shooterSheet.enemies.Sort((e1, e2) => (shooterPos-e1.position).sqrMagnitude.CompareTo((shooterPos-e2.position).sqrMagnitude));
		if(!shooterSheet.enemies[0].shootable){
			return;
		}

		Vector3 enemyPos = shooterSheet.enemies[0].position;
		Vector3 explosionPos = new Vector3(enemyPos.x-1 + Random.Range(0, 3), enemyPos.y-1 + Random.Range(0,3));
		int layerMask0 = 1 << 8;
		int layerMask1 = 1 << 9;
		int layerMask = layerMask0 | layerMask1;		//friendly fire is possible
		
		Collider[] unitsAffected = Physics.OverlapSphere(explosionPos, 1.5f, layerMask);
		
		int hitDieCount = shooterSheet.hitDieCount;
		int hitDieType = shooterSheet.hitDieType;
		int hitDie = 0;

		for(int i=0; i < hitDieCount; i++){
			hitDie += Random.Range(1, hitDieType+1);
		}

		int shrapnelDmg;
		for(int i=0; i < unitsAffected.Length; i++){
			shrapnelDmg = Random.Range(1, hitDie+1);
			if(shrapnelDmg > unitsAffected[i].gameObject.GetComponent<GCS>().armorClass){
				shooterSheet.commandHub.CmdHit(unitsAffected[i].gameObject);
			}
			hitDie -= shrapnelDmg;
			if(hitDie <= 0){
				break;
			}
		}
		for(int i=0; i < unitsAffected.Length; i++){
			unitsAffected[i].gameObject.GetComponent<GCS>().Alert(shooter);
		}

		AlertEnemiesByGunNoise(shooter, explosionPos);

	}
}
