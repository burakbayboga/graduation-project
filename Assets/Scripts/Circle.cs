using System.Collections;
using UnityEngine;

//Author: MarkPixel at forum.unity.com joined at: apr 15, 2010
public class Circle : MonoBehaviour {

	public int segments;
	float xRadius;
	float yRadius;
	LineRenderer line;

	void Start(){
		xRadius = gameObject.transform.parent.gameObject.GetComponent<GCS>().rangeCollider.radius;
		yRadius = gameObject.transform.parent.gameObject.GetComponent<GCS>().rangeCollider.radius;
		line = gameObject.GetComponent<LineRenderer>();
		line.useWorldSpace = false;
	}

	public void Show(){
		line.SetVertexCount(segments+1);
		float x;
		float y;
		float z = 0f;
		float angle = 20f;

		for(int i=0; i < segments+1; i++){
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
			y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;
			
			line.SetPosition(i, new Vector3(x, y, z));
			angle += 360f/segments;
		}
	}

	public void Hide(){
		line.SetVertexCount(0);
	}

}
