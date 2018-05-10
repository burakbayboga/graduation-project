using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour {

	LineRenderer line;

	void Start(){
		line = gameObject.GetComponent<LineRenderer>();
	}

	public void Show(Vector3 target, Vector3 source){
		Vector3 direction = (target - source).normalized;
		line.SetVertexCount(2);
		line.SetPosition(0, source + direction*2.0f + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f));
		line.SetPosition(1, source + direction*3.0f + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f));
	}

	public void Hide(){
		line.SetVertexCount(0);
	}

}
