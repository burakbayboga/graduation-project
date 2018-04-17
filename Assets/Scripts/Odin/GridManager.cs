using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	public GameObject groundPrefab;

	void Start(){
		GameObject ground;
		//FIX HARDCODED MAPSIZE
		for(int i=0; i < 100/10; i++){
			for(int j=0; j < 100/10; j++){
				ground = Instantiate(groundPrefab, new Vector3(i*5 + 2.5f, j*5 + 2.5f, 0f), Quaternion.identity);
				ground.GetComponent<Ground>().InitializeGridCoords();
				ground.transform.parent = gameObject.transform;
			}
		}
	}
}