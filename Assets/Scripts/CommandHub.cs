using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandHub : NetworkBehaviour {

	public GameObject[] deployableUnits;
	public GameObject odin;


	[SyncVar]
	public int client;
	public string IS;
	public List<GameObject> deployedUnits;
	public List<GameObject> spotters;
	[SyncVar]
	public int emptySlot;
	public NetworkConnection conn;
	

	char MUI;
	string ISUnits;
	string ISOpCode;
	string ISTargetCoords;
	int targetX;
	int targetY;

	void Start(){
		
		IS = "";
		MUI = '*';
		Instantiate(odin, new Vector3(0f, 0f, 0f), Quaternion.identity);
		deployedUnits = new List<GameObject>();
		spotters = new List<GameObject>();
	}

	[Command]
	public void CmdHit(GameObject unitHit){
		unitHit.GetComponent<GCS>().personCount--;
		if(unitHit.GetComponent<GCS>().personCount > 0){
			unitHit.GetComponent<GCS>().RpcLickWound(unitHit.GetComponent<GCS>().personCount);
		}
		else{
			unitHit.GetComponent<GCS>().RpcHandleSelfDeath();
		}
	}

	[Command]
	public void CmdApplyStress(GameObject unitStressed, float incomingStress){
		//unitStressed.GetComponent<GCS>().RpcApplyStress(incomingStress);
		unitStressed.GetComponent<GCS>().currentStress += incomingStress * ((100 - unitStressed.GetComponent<GCS>().stressResilience) / 100);

	}
	
	public void ClearSpottersList(){
		for(int i=0; i < spotters.Count; i++){
			if(spotters[i] == null){
				spotters.RemoveAt(i);
				i--;
			}
		}
	}

	void Update(){
		if(!isLocalPlayer){
			return;
		}

		foreach(char c in Input.inputString){
			if(c == '\r'){
				ParseCommand();
				CmdExecute(ISOpCode, ISUnits, targetX, targetY);
				IS = "";
				ISUnits = "";
				ISOpCode = "";
				ISTargetCoords = "";
			}
			else if(c == '\\'){
				IS = "";
			}
			else{
				IS += c;
			}
		}
	}

	

	[Command]
	void CmdExecute(string ISOpCode, string ISUnits, int targetX, int targetY){
		if(ISOpCode == "deploy"){
			GameObject newUnit;
			if(ISUnits == "marine"){
				newUnit = Instantiate(deployableUnits[0], new Vector3(targetX, targetY), Quaternion.identity);
			}
			/*else if(ISUnits == "marine1"){
				newUnit = Instantiate(deployableUnits[1], new Vector3(targetX, targetY), Quaternion.identity);
			}*/
			else if(ISUnits == "sniper"){
				newUnit = Instantiate(deployableUnits[1], new Vector3(targetX, targetY), Quaternion.identity);
			}
			else if(ISUnits == "spotter"){
				newUnit = Instantiate(deployableUnits[2], new Vector3(targetX, targetY), Quaternion.identity);
			}
			else{
				return;
			}

			newUnit.GetComponent<GCS>().client = client;
			//NetworkServer.SpawnWithClientAuthority(newUnit, conn);
			NetworkServer.Spawn(newUnit);
		}
		else if(ISOpCode == "move"){
			List<int> unitIDs = ParseISUnits(ISUnits);
			for(int i=0; i < unitIDs.Count; i++){
				//Debug.Log(deployedUnits[unitIDs[i]].GetComponent<GCS>().moveSpeed);
				//deployedUnits[unitIDs[i]].GetComponent<GCS>().mover.GetMoving(targetX, targetY);
				GCS unitCommanded = deployedUnits[unitIDs[i]].GetComponent<GCS>();
				unitCommanded.currentCommand = ISOpCode;
				unitCommanded.currentTarget = new Vector3(targetX, targetY);
				unitCommanded.originalTarget = new Vector3(targetX, targetY);
			}
		}
		else if(ISOpCode == "hold_position"){
			List<int> unitIDs = ParseISUnits(ISUnits);
			for(int i=0; i < unitIDs.Count; i++){
				//Debug.Log(deployedUnits[unitIDs[i]].GetComponent<GCS>().moveSpeed);
				//deployedUnits[unitIDs[i]].GetComponent<GCS>().mover.GetMoving(targetX, targetY);
				GCS unitCommanded = deployedUnits[unitIDs[i]].GetComponent<GCS>();
				unitCommanded.currentCommand = ISOpCode;
				unitCommanded.currentTarget = new Vector3(targetX, targetY);
				unitCommanded.originalTarget = new Vector3(targetX, targetY);
			}
		}
	}

	List<int> ParseISUnits(string ISUnits){
		string[] splitISUnits = ISUnits.Split(',');
		List<int> unitIDs = new List<int>();
		for(int i=0; i < splitISUnits.Length; i++){
			unitIDs.Add(int.Parse(splitISUnits[i]));
		}
		return unitIDs;
	}

	void ParseCommand(){
		int i = 0;
		
		if(IS[0] == MUI){
			for(i = i+2; i < IS.Length; i++){
				if(IS[i] == ')'){
					break;
				}
				ISUnits += IS[i];
			}
			i += 2;
		}

		else{
			for(; i < IS.Length; i++){
				if(IS[i] == ' '){
					break;
				}
				ISUnits += IS[i];
			}
			i++;
		}
		
		for(; i < IS.Length; i++){
			if(IS[i] == ' '){
				break;
			}
			ISOpCode += IS[i];
		}

		for(i = i+1; i < IS.Length; i++){
			ISTargetCoords += IS[i];
		}

		string[] splitISTargetCoords = ISTargetCoords.Split(',');
		targetX = int.Parse(splitISTargetCoords[0]);
		targetY = int.Parse(splitISTargetCoords[1]);
	}

	void OnGUI(){
		if(!isLocalPlayer){
			return;
		}
		GUI.TextField(new Rect(80, 400, 200, 20), IS);
	}

	

	
}
