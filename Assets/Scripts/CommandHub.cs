using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandHub : NetworkBehaviour {

	public GameObject[] deployableUnits;
	public GameObject odinPrefab;


	[SyncVar]
	public int client;
	public string IS;
	public List<GameObject> deployedUnits;
	public List<GameObject> spotters;
	[SyncVar]
	public int emptySlot;
	public NetworkConnection conn;
	public GameObject _odinToSpawn;
	public GameObject odin;
	[SyncVar]
	public int resource;

	char MUI;
	string ISUnits;
	string ISOpCode;
	string ISTargetCoords;
	int targetX;
	int targetY;

	public ErrorText errorText;


	void Start(){
		resource = 100;
		StartCoroutine(GainResource());
		IS = "";
		MUI = '*';
		
		deployedUnits = new List<GameObject>();
		spotters = new List<GameObject>();
		if(client == 0 && isLocalPlayer){
			_odinToSpawn = Instantiate(odinPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
			NetworkServer.Spawn(_odinToSpawn);
		}
		odin = GameObject.FindGameObjectsWithTag("odin")[0];
	}

	IEnumerator GainResource(){
		while(true){
			yield return new WaitForSeconds(5f);
			if(resource >= 85){
				resource = 100;
			}
			else{
				resource += 15;
			}
		}
	}

	[ClientRpc]
	public void RpcSpendResource(int amountSpent){
		resource -= amountSpent;
	}

	[ClientRpc]
	public void RpcDisplayErrorText(){
		if(!isLocalPlayer){
			return;
		}
		errorText.GenerateErrorText();
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

	void ClearDeployedUnitsList(){
		for(int i=0; i < deployedUnits.Count; i++){
			if(deployedUnits[i] == null){
				deployedUnits.RemoveAt(i);
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
				//ParseCommand();
				//CmdExecute(ISOpCode, ISUnits, targetX, targetY, gameObject);
				CmdExecuteTest(IS, gameObject);
				IS = "";
				ISUnits = "";
				ISOpCode = "";
				ISTargetCoords = "";
			}
			else if(c == '\b'){
				if(IS.Length != 0){
					IS = IS.Substring(0, IS.Length - 1);
				}
			}
			else if(c == '\\'){
				IS = "";
			}
			else{
				IS += c;
			}
		}
	}

	int UnitIndexInDeployable(string ISUnit){
		if(ISUnit == "marine"){
			return 0;
		}
		if(ISUnit == "sniper"){
			return 1;
		}
		if(ISUnit == "spotter"){
			return 2;
		}
		if(ISUnit == "bomber"){
			return 3;
		}
		return -1;
	}

	int ParamMultipleDeploy(List<string> parameters){
		for(int i=0; i < parameters.Count; i++){
			if(parameters[i][0] == 'c'){
				return i;
			}
		}
		return -1;
	}

	List<string> ParseParameters(string[] splitIS){
		List<string> parameters = new List<string>();
		for(int i=3; i < splitIS.Length; i++){
			string temp = "";
			for(int j=1; j < splitIS[i].Length; j++){
				temp += splitIS[i][j];
			}
			parameters.Add(temp);
		}
		return parameters;
	}

	int ResourceToSpend(int unitIndex, int resource, int multipleDeployCount){
		if(unitIndex == 0 && resource >= 20*multipleDeployCount){
			return 20;
		}
		if(unitIndex == 1 && resource >= 45*multipleDeployCount){
			return 45;
		}
		if(unitIndex == 2 && resource >= 15*multipleDeployCount){
			return 15;
		}
		if(unitIndex == 3 && resource >= 30*multipleDeployCount){
			return 30;
		}
		return -1;
	}

	[Command]
	void CmdExecuteTest(string IS, GameObject player){
		string[] splitIS = IS.Split(' ');
		List<string> parametersList = new List<string>();
		if(splitIS.Length > 3){
			parametersList = ParseParameters(splitIS);
		}
		string ISUnits = splitIS[0];
		string ISOpCode = splitIS[1];
		string[] splitISTargetCoords = splitIS[2].Split(',');
		int targetX = int.Parse(splitISTargetCoords[0]);
		int targetY = int.Parse(splitISTargetCoords[1]);

		if(ISOpCode == "deploy"){
			
			GameObject newUnit;
			//PI: parameter index
			int multipleDeployPI = ParamMultipleDeploy(parametersList);
			int multipleDeployCount;
			if(multipleDeployPI == -1){
				multipleDeployCount = 1;
			}
			else{
				string multipleDeployString = "";
				for(int i=1; i < parametersList[multipleDeployPI].Length; i++){
					multipleDeployString += parametersList[multipleDeployPI][i];
				}
				multipleDeployCount = int.Parse(multipleDeployString);
			}
			int unitIndex = UnitIndexInDeployable(ISUnits);
			//invalid unit string
			if(unitIndex == -1){
				errorText.GenerateErrorText();
				return;
			}
			int spendingPerUnit = ResourceToSpend(unitIndex, player.GetComponent<CommandHub>().resource, multipleDeployCount);
			//not enough resource
			if(spendingPerUnit == -1){
				errorText.GenerateErrorText();
				return;				
			}
			StartCoroutine(DeployCoroutine(unitIndex, player, spendingPerUnit, multipleDeployCount, targetX, targetY));
		}
		else if(ISOpCode == "move"){
			List<int> unitIDs = ParseISUnits(ISUnits);
			for(int i=0; i < unitIDs.Count; i++){
				GCS unitCommanded = deployedUnits[unitIDs[i]].GetComponent<GCS>();
				unitCommanded.currentCommand = ISOpCode;
				unitCommanded.gridTarget = new Vector2(targetX, targetY);
			}
		}
		else if(ISOpCode == "hold"){
			List<int> unitIDs = ParseISUnits(ISUnits);
			for(int i=0; i < unitIDs.Count; i++){
				GCS unitCommanded = deployedUnits[unitIDs[i]].GetComponent<GCS>();
				unitCommanded.currentCommand = "hold_position";
				unitCommanded.gridTarget = new Vector2(targetX, targetY);
			}
		}
	}


	IEnumerator DeployCoroutine(int unitIndex, GameObject player, int spendingPerUnit, int unitCount, int gridX, int gridY){
		for(int i=0; i < unitCount; i++){
			Vector3 deployPosition = GridToRW.GetGridToRW(gridX, gridY, odin.GetComponent<AStarMap>());
			CmdDeployUnit(unitIndex, player, deployPosition, spendingPerUnit);
			yield return new WaitForSeconds(0.2f);
			//yield return null;
		}
	}

	[Command]
	void CmdDeployUnit(int unitIndex, GameObject player, Vector3 deployPosition, int spendingPerUnit){
		GameObject newUnit = Instantiate(deployableUnits[unitIndex], deployPosition, Quaternion.identity);
		player.GetComponent<CommandHub>().RpcSpendResource(spendingPerUnit);
		newUnit.GetComponent<GCS>().client = client;
		if(client == 0){
			newUnit.layer = 8;
		}
		else{
			newUnit.layer = 9;
		}
		NetworkServer.Spawn(newUnit);
	}

	[Command]
	void CmdExecute(string ISOpCode, string ISUnits, int targetX, int targetY, GameObject player){
		if(ISOpCode == "deploy"){
			GameObject newUnit;
			Vector3 deployPosition = GridToRW.GetGridToRW(targetX, targetY, odin.GetComponent<AStarMap>());
			if(ISUnits == "marine"){
				if(player.GetComponent<CommandHub>().resource >= 20){
					newUnit = Instantiate(deployableUnits[0], deployPosition, Quaternion.identity);
					player.GetComponent<CommandHub>().RpcSpendResource(20);
				}
				else{
					errorText.GenerateErrorText();
					return;
				}
			}
			else if(ISUnits == "sniper"){
				if(player.GetComponent<CommandHub>().resource >= 45){
					newUnit = Instantiate(deployableUnits[1], deployPosition, Quaternion.identity);
					player.GetComponent<CommandHub>().RpcSpendResource(45);
				}
				else{
					errorText.GenerateErrorText();
					return;
				}
			}
			else if(ISUnits == "spotter"){
				if(player.GetComponent<CommandHub>().resource >= 15){	
					newUnit = Instantiate(deployableUnits[2], deployPosition, Quaternion.identity);
					player.GetComponent<CommandHub>().RpcSpendResource(15);
				}
				else{
					errorText.GenerateErrorText();
					return;
				}
			}
			//replace bomber -> explosives
			else if(ISUnits == "bomber"){
				if(player.GetComponent<CommandHub>().resource >= 30){	
					newUnit = Instantiate(deployableUnits[3], deployPosition, Quaternion.identity);
					player.GetComponent<CommandHub>().RpcSpendResource(30);
				}
				else{
					errorText.GenerateErrorText();
					return;
				}
			}
			else{
				errorText.GenerateErrorText();
				return;
			}

			newUnit.GetComponent<GCS>().client = client;
			//NetworkServer.SpawnWithClientAuthority(newUnit, conn);
			NetworkServer.Spawn(newUnit);
		}
		else if(ISOpCode == "move"){
			ClearDeployedUnitsList();
			List<int> unitIDs = ParseISUnits(ISUnits);
			for(int i=0; i < unitIDs.Count; i++){
				//Debug.Log(deployedUnits[unitIDs[i]].GetComponent<GCS>().moveSpeed);
				//deployedUnits[unitIDs[i]].GetComponent<GCS>().mover.GetMoving(targetX, targetY);
				GCS unitCommanded = deployedUnits[unitIDs[i]].GetComponent<GCS>();
				unitCommanded.currentCommand = ISOpCode;
				unitCommanded.gridTarget = new Vector2(targetX, targetY);
				//unitCommanded.currentTarget = new Vector3(targetX, targetY);
				//unitCommanded.originalTarget = new Vector3(targetX, targetY);
			}
		}
		else if(ISOpCode == "hold_position"){
			ClearDeployedUnitsList();
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
		else{
			errorText.GenerateErrorText();
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
		GUI.Label(new Rect(80, 20, 100, 20), "Resources: " + resource);
		GUI.TextField(new Rect(80, 400, 200, 20), IS);
	}	
}