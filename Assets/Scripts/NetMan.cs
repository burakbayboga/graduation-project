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
		//InitializeGround();
		Instantiate(gridManagerPrefab, Vector3.zero, Quaternion.identity);

	}

	/*void InitializeGround(){
		GameObject ground;
		//FIX HARDCODED MAPSIZE
		for(int i=0; i < 100/10; i++){
			for(int j=0; j < 100/10; j++){
				ground = Instantiate(groundPrefab, new Vector3(i*5 + 2.5f, j*5 + 2.5f, 0f), Quaternion.identity);
				ground.GetComponent<Ground>().InitializeGridCoords();
			}
		}
	}*/

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){
		GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		player.GetComponent<CommandHub>().client = nextPlayer;
		player.GetComponent<CommandHub>().emptySlot = myID;
		player.GetComponent<CommandHub>().conn = conn;
		nextPlayer++;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	
}
