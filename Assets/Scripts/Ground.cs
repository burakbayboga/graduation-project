using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour{

	

	TextMesh gridCoord;

	public void InitializeGridCoords(){
		gridCoord = GetComponentInChildren<TextMesh>();
		//gridCoord.text = new Vector3((transform.position.x - 2.5f)/5f, (transform.position.y - 2.5f)/5f, 0f).ToString();
		gridCoord.text = ((transform.position.x - 2.5f)/5f).ToString() + "," + ((transform.position.y - 2.5f)/5f).ToString();
	}

	
}
