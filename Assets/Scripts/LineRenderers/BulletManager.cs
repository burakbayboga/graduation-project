using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

	public GameObject bulletPrefab;
	public List<GameObject> bullets;
	public List<bool> freeBullets;

	void Start(){
		bullets = new List<GameObject>();
		freeBullets = new List<bool>();

		for(int i=0; i < 100; i++){
			bullets.Add(Instantiate(bulletPrefab, new Vector3(-100f, -100f, 0f), Quaternion.identity));
			freeBullets.Add(true);
			bullets[i].transform.parent = gameObject.transform;
		}
	}

	public int GetBulletIndex(){
		for(int i=0; i < 100; i++){
			if(freeBullets[i]){
				return i;
			}
		}
		return -1;
	}
}