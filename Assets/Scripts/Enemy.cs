using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy {

	public GameObject unit;
	public GCS unitSheet;
	public Vector3 position;
	public bool shootable;
	public bool canShootMe;
	public int numberOfAlertTimers;

	public Enemy(GameObject _unit){
		unit = _unit;
		unitSheet = unit.GetComponent<GCS>();
		position = unit.transform.position;
		shootable = false;
		canShootMe = false;
		numberOfAlertTimers = 0;
	}
	
}
