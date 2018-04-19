using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorText : MonoBehaviour {

	TextMesh errorText;

	void Start(){
		errorText = GetComponent<TextMesh>();
		errorText.text = "";
	}

	public void GenerateErrorText(){
		StartCoroutine(DisplayText());
	}

	IEnumerator DisplayText(){
		errorText.text = "Invalid Command";
		yield return new WaitForSeconds(2.0f);
		errorText.text = "";
	}

}
