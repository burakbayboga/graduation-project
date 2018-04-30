using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetMan : NetworkManager {

	public int nextPlayer = 0;
	public GameObject gridManagerPrefab;

	int myID;

	void Start(){
		myID = gameObject.GetInstanceID();
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		for(int i=0; i < obstacles.Length; i++){
			obstacles[i].GetComponent<GOS>().emptySlot = myID;
			obstacles[i].GetComponent<GOS>().AssignPossibilityValues();
			CoverSpotsCollider[] coverSpots = obstacles[i].GetComponentsInChildren<CoverSpotsCollider>();
			for(int j=0; j < coverSpots.Length; j++){
				coverSpots[j].emptySlot = myID;
			}
		}
		Instantiate(gridManagerPrefab, Vector3.zero, Quaternion.identity);
	}


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){
		GameObject player = Instantiate(playerPrefab, new Vector3(20f, 50f, 0f), Quaternion.identity);
		player.GetComponent<CommandHub>().client = nextPlayer;
		player.GetComponent<CommandHub>().emptySlot = myID;
		player.GetComponent<CommandHub>().conn = conn;
		nextPlayer++;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	
}
