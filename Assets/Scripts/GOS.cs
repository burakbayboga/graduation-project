using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOS : MonoBehaviour {

	public int coverLevel;
	public List<Vector3> coordinates;
	public List<int> possibilities;
	public int emptySlot;

	void Start(){
		coordinates = new List<Vector3>();
		//0
		coordinates.Add(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1));
		//1
		coordinates.Add(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1));
		//2
		coordinates.Add(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y));
		//3
		coordinates.Add(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y - 1));
		//4
		coordinates.Add(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1));
		//5
		coordinates.Add(new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y - 1));
		//6
		coordinates.Add(new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y));
		//7
		coordinates.Add(new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y + 1));
	}

	public void AssignPossibilityValues(){
		possibilities = new List<int>();
		for(int i=0; i < 8; i++){
			possibilities.Add(emptySlot);
		}

		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(gameObject.transform.position, 1.5f, layerMask);
		for(int i=0; i < coverColliders.Length; i++){
			for(int j=0; j < coordinates.Count; j++){
				if(coverColliders[i].gameObject.transform.position == coordinates[j]){
					possibilities[j] = coverColliders[i].gameObject.GetInstanceID();
					break;
				}
			}
		}
	}
}
