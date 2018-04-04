using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BaseFire {

	GameObject enemy;
	GameObject shooter;
	Vector3 enemyPos;
	Vector3 shooterPos;
	GCS shooterSheet;
	GCS enemySheet;

	public override void Fire(GameObject _shooter){
		shooter = _shooter;
		shooterPos = shooter.transform.position;
		shooterSheet = shooter.GetComponent<GCS>();

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

		enemy = shooterSheet.enemies[0].unit;
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


		enemySheet.Alert(shooter);
		AlertEnemiesByGunNoise(shooter, shooterPos);
		shooterSheet.commandHub.CmdApplyStress(enemy, hitDie);

		if(hitDie >= enemySheet.armorClass){
			shooterSheet.commandHub.CmdHit(enemy);
		}


		

	}
	
}
