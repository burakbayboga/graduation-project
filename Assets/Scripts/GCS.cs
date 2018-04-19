using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GCS : NetworkBehaviour {

	public Material[] materials;
	public BaseFire baseFire;
	public SphereCollider rangeCollider;


	public float moveSpeed;
	public int hitDieType;
	public int hitDieCount;
	public float fireRate;
	public int armorClass;
	public float gunNoiseReach;
	public float stressResilience;
	[SyncVar]
	public float currentStress;
	[SyncVar]
	public int personCount;
	public bool hiding;
	public string unitType;


	[SyncVar]
	public int client;
	public Mover mover;
	public GameObject odin;
	public CommandHub commandHub;
	public CommandHub enemyCommandHub;
	public List<Enemy> enemies;
	public string currentCommand;
	public TextMesh personCountText;
	public bool localUnit;

	public Vector3 currentTarget;
	public Vector3 originalTarget;

	public Vector2 gridTarget;
	public Vector3 rWTarget;
	public bool sideTracked;

	public List<GameObject> spottedUnits;
	

	

	void Start(){
		/*string testWithParameter = "marine deploy 5,5 -3";
		string[] splitTestWithParamater = testWithParameter.Split(' ');
		string testWithoutParameter = "marine deploy 5,5";
		string[] splitTestWithoutParamater = testWithoutParameter.Split(' ');
		Debug.Log(splitTestWithParamater.Length);
		for(int i=0; i < splitTestWithParamater.Length; i++){
			Debug.Log(splitTestWithParamater[i]);
		}
		Debug.Log(splitTestWithoutParamater.Length);
		for(int i=0; i < splitTestWithoutParamater.Length; i++){
			Debug.Log(splitTestWithoutParamater[i]);
		}*/

		gridTarget = new Vector2(-1f, -1f);
		rWTarget = new Vector2(-1f, -1f);
		sideTracked = false;
		GetComponent<Renderer>().sharedMaterial = materials[client];
		odin = GameObject.FindGameObjectsWithTag("odin")[0];
		mover = GetComponent<Mover>();
		personCountText = GetComponentInChildren<TextMesh>();
		personCountText.text = personCount.ToString();

		GameObject[] tempHubs = GameObject.FindGameObjectsWithTag("Player");
		commandHub = tempHubs[client].GetComponent<CommandHub>();
		
		if(client == 0){
			enemyCommandHub = tempHubs[1].GetComponent<CommandHub>();
		}
		else{
			enemyCommandHub = tempHubs[0].GetComponent<CommandHub>();
		}
		
		commandHub.deployedUnits.Add(gameObject);
		gameObject.layer = client + 8;
		enemies = new List<Enemy>();
		spottedUnits = new List<GameObject>();
		currentCommand = "idle";
		localUnit = commandHub.isLocalPlayer;
		
		if(!isServer/*localUnit*/){
			return;
		}
		
		InitBT();
		StartCoroutine(ShootCoroutine());
	}

	

	[ClientRpc]
	public void RpcLickWound(int newPersonCount){
		//if(personCount <= 0){
		//	HandleSelfDeath();
		//}
		//else{
			personCountText.text = newPersonCount.ToString();
		//}
	}

	[ClientRpc]
	public void RpcHandleSelfDeath(){
		int layerMask = 1 << 10;
		Collider[] coverColliders = Physics.OverlapSphere(gameObject.transform.position, 1.5f, layerMask);
		GOS currentCover;
		int myID = gameObject.GetInstanceID();
		for(int i=0; i < coverColliders.Length; i++){
			currentCover = coverColliders[i].gameObject.GetComponent<GOS>();
			for(int j=0; j < 8; j++){
				if(currentCover.possibilities[j] == myID){
					currentCover.possibilities[j] = commandHub.emptySlot;
					break;
				}
			}
		}
		if(isServer){
			NetworkServer.Destroy(gameObject);
		}
	}

	/*[ClientRpc]
	public void RpcApplyStress(float incomingStress){
		currentStress += incomingStress * ((100 - stressResilience) / 100); 
	}*/	

	/*public void LickWound(){
		if(personCount <= 0){
			HandleSelfDeath();
		}
		else{
			personCountText.text = personCount.ToString();
		}
	}*/



	

	void InitBT(){
		BT bT = new BT();
		bT.unit = this;
		bT.emptySlot = commandHub.emptySlot;
		bT.Initialize();
		StartCoroutine(Tick(bT));
	}

	IEnumerator Tick(BT bT){
		while(true){
			bT.root.Execute();
			yield return new WaitForSeconds(0.3f);
		}
	}

	IEnumerator ShootCoroutine(){
		while(true){
			//GetComponent<Renderer>().material.color = Color.black; 
			if(enemies.Count > 0){
				baseFire.Fire(gameObject);
			}
			yield return new WaitForSeconds(fireRate);
		}
	}

	public void ClearEnemyList(){
		for(int i=0; i < enemies.Count; i++){
			if(enemies[i].unit == null){
				enemies.RemoveAt(i);
				i--;
			}
		}
	}

	public void ClearSpottedUnitsList(){
		for(int i=0; i < spottedUnits.Count; i++){
			if(spottedUnits[i] == null){
				spottedUnits.RemoveAt(i);
				i--;
			}
		}
	}

	

	public void Alert(GameObject _enemyUnit){
		int index = IndexOfEnemy(_enemyUnit);
		if(index == -1){
			Enemy newEnemy = new Enemy(_enemyUnit);
			//newEnemy.canShootMe = true;
			enemies.Add(newEnemy);
			StartCoroutine(AlertTimer(newEnemy));
		}
		else{
			//enemies[index].canShootMe = true;
			StartCoroutine(AlertTimer(enemies[index]));
		}
	}

	IEnumerator AlertTimer(Enemy enemy){
		enemy.canShootMe = true;
		enemy.numberOfAlertTimers++;
		yield return new WaitForSeconds(4.0f);
		enemy.numberOfAlertTimers--;
		if(enemy.numberOfAlertTimers == 0){
			enemy.canShootMe = false;
		}
	}

	public int IndexOfEnemy(GameObject comparedUnit){
		for(int i=0; i < enemies.Count; i++){
			if(comparedUnit == enemies[i].unit){
				return i;
			}
		}
		return -1;
	}


	
}
