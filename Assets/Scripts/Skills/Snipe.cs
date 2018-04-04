using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : BaseFire {

	GameObject shooter;
	GameObject enemy;
	Vector3 shooterPos;
	Vector3 enemyPos;
	GCS shooterSheet;
	GCS enemySheet;

	public Snipe(){

	}

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
		shooterSheet.commandHub.ClearSpottersList();
		List<GameObject> spotters = shooterSheet.commandHub.spotters;
		List<GameObject> spottedUnits;
		int index = -1;
		bool targetFound = false;

		for(int i=0; i < shooterSheet.enemies.Count; i++){
			for(int j=0; j < spotters.Count; j++){
				spotters[j].GetComponent<GCS>().ClearSpottedUnitsList();
				spottedUnits = spotters[j].GetComponent<GCS>().spottedUnits;
				for(int k=0; k < spottedUnits.Count; k++){
					if(shooterSheet.enemies[i].unit == spottedUnits[k] && shooterSheet.enemies[i].shootable){
						index = i;
						targetFound = true;
						break;
					}
				}
				if(targetFound){
					break;
				}
			}
			if(targetFound){
				break;
			}
		}
		if(index == -1){
			return;
		}

		enemy = shooterSheet.enemies[index].unit;
		enemyPos = enemy.transform.position;
		enemySheet = enemy.GetComponent<GCS>();

		int hitDieCount = shooterSheet.hitDieCount;
		int hitDieType = shooterSheet.hitDieType;
		int hitDie = 0;

		for(int i=0; i < hitDieCount; i++){
			hitDie += Random.Range(1, hitDieType+1);
		}

		int enemyCoverLevel = EnemyCoverLevel(enemyPos, shooterPos);
		if(enemyCoverLevel == 1){
			hitDie -= hitDieType / 3;
		}
		else if(enemyCoverLevel == 2){
			hitDie -= hitDieType / 2;
		}

		if(hitDie >= enemySheet.armorClass){
			shooterSheet.commandHub.CmdHit(enemy);
		}

		enemySheet.Alert(shooter);
		AlertEnemiesByGunNoise(shooter, shooterPos);

	}
	
}
