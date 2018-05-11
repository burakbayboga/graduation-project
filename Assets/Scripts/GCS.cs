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
	public string stressResolution;
	[SyncVar]
	public int personCount;
	public int personCountBase;
	public bool hiding;
	public string unitType;


	[SyncVar]
	public int client;
	public Mover mover;
	public GameObject odin;
	public CommandHub commandHub;

	public List<Enemy> enemies;
	public string currentCommand;
	public TextMesh personCountText;
	public TextMesh deployedIndexText;
	public GameObject rangeLineRenderer;

	public bool localUnit;

	public Vector3 currentTarget;
	public Vector3 originalTarget;

	public Vector2 gridTarget;
	public Vector3 rWTarget;
	public bool sideTracked;
	public bool diffusing;
	public bool diffusionFail;
	public bool charging;
	public bool chargeFail;

	public List<GameObject> spottedUnits;
	

	

	void Start(){
		baseFire = Instantiate(baseFire, Vector3.zero, Quaternion.identity);
		personCountBase = personCount;
		stressResolution = "calm";
		diffusing = false;
		diffusionFail = false;
		charging = false;
		chargeFail = false;
		gridTarget = new Vector2(-1f, -1f);
		rWTarget = new Vector2(-1f, -1f);
		sideTracked = false;
		GetComponent<Renderer>().sharedMaterial = materials[client];
		odin = GameObject.FindGameObjectsWithTag("odin")[0];
		baseFire.odin = odin;
		baseFire.bulletManager = odin.GetComponent<BulletManager>();
		baseFire.shooterObject = gameObject;
		mover = GetComponent<Mover>();
		//personCountText = GetComponentInChildren<TextMesh>();
		//personCountText = transform.GetChild(1).GetComponent<TextMesh>();
		personCountText.text = personCount.ToString();
		//deployedIndexText = transform.GetChild(2).GetComponent<TextMesh>();

		commandHub = GameObject.FindGameObjectsWithTag("Player")[client].GetComponent<CommandHub>();		
		commandHub.deployedUnits.Add(gameObject);
		enemies = new List<Enemy>();
		spottedUnits = new List<GameObject>();
		currentCommand = "idle";
		localUnit = commandHub.isLocalPlayer;
		
		if(!isServer){
			return;
		}
		
		InitBT();
		StartCoroutine(ShootCoroutine());
		StartCoroutine(RefreshCoroutine());
	}

	

	[ClientRpc]
	public void RpcShowRange(){
		if(!localUnit){
			return;
		}
		StartCoroutine(ShowRangeCoroutine());
	}

	IEnumerator ShowRangeCoroutine(){
		Circle circle = rangeLineRenderer.GetComponent<Circle>();
		for(int i=0; i < 4; i++){
			circle.Show();
			yield return new WaitForSeconds(0.2f);
			circle.Hide();
			yield return new WaitForSeconds(0.2f);
		}
	}

	IEnumerator RefreshCoroutine(){
		BTNode underAttack = new BeingShotAt(this);
		while(true){
			if(underAttack.Execute() == -1 && personCount < personCountBase){
				commandHub.CmdRefreshUnit(gameObject);
			}
			yield return new WaitForSeconds(10f);
		}
	}

	[ClientRpc]
	public void RpcLickWound(int newPersonCount){
		personCountText.text = newPersonCount.ToString();
	}

	[ClientRpc]
	public void RpcUpdateUnitIndex(int index){
		deployedIndexText.text = index.ToString();
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
		commandHub.deployedUnits.Remove(gameObject);
		if(isServer){
			NetworkServer.Destroy(gameObject);
		}
	}

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

	public void UpdateEnemyPositions(){
		for(int i=0; i < enemies.Count; i++){
			enemies[i].position = enemies[i].unit.transform.position;
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
			enemies.Add(newEnemy);
			StartCoroutine(AlertTimer(newEnemy));
		}
		else{
			StartCoroutine(AlertTimer(enemies[index]));
		}
	}

	public void AlertByGunNoise(GameObject _enemyUnit){
		int index = IndexOfEnemy(_enemyUnit);
		if(index == -1){
			Enemy newEnemy = new Enemy(_enemyUnit);
			enemies.Add(newEnemy);
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