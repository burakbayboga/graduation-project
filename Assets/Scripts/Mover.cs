using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	AStar pathFinder;
	float moveSpeed;
	GCS unit;
	Coroutine moveCoroutine;
	bool coroutineRunning;

	bool replanningTimerRunning;
	Coroutine replanningTimerCoroutine;
	

	public AStarNode[,] map;
	Stack<Vector2> path;
	bool speedCooldown;

	Vector2 ultTarget;


	
	void Start(){
		unit = GetComponent<GCS>();
		moveSpeed = unit.moveSpeed;
		coroutineRunning = false;
		pathFinder = new AStar();
		map = unit.odin.GetComponent<AStarMap>().map;
		path = new Stack<Vector2>();
		pathFinder.map = map;
		speedCooldown = false;
		StartCoroutine(InfiniteMoveCoroutine());
	}

	

	public void MultiplyMoveSpeed(float multiplier){
		if(speedCooldown){
			return;
		}
		float oldMoveSpeed = moveSpeed;
		moveSpeed /= multiplier;
		speedCooldown = true;
		StartCoroutine(MultiplyMoveSpeedTimer(oldMoveSpeed));
	}

	IEnumerator MultiplyMoveSpeedTimer(float oldMoveSpeed){
		yield return new WaitForSeconds(2.0f);
		moveSpeed = oldMoveSpeed;
		yield return new WaitForSeconds(8.0f);
		speedCooldown = false;
	}



	public void GetMovingRW(Vector2 target){
		ultTarget = target;
		/*if(unit.rWTarget.x == -1f){
			unit.rWTarget = new Vector3(ultTarget.x, ultTarget.y);
		}*/
		GetPath(ultTarget);
		//StartMovement();
	}

	public void GetMovingGrid(int _xGrid, int _yGrid){
		//Override();


		Vector2 tempTarget = GridToRW.GetGridToRW(_xGrid, _yGrid, unit.odin.GetComponent<AStarMap>());
		if(tempTarget.x == -1f){
			//FIX ME PLS
			Debug.Log("grid full");
		}
		unit.rWTarget = new Vector3(tempTarget.x, tempTarget.y);
		GetMovingRW(tempTarget);
		//moveCoroutine = StartCoroutine(MoveCoroutine(/*_x, _y*/));
		//GetPath(ultTarget);
		//StartMovement();
	}

	public void RunForCover(int _x, int _y){
		ultTarget = new Vector2(_x, _y);
		GetPath(ultTarget);
		//StartMovement();
	}

	void StartMovement(){
		Override();
		moveCoroutine = StartCoroutine(MoveCoroutine());
	}

	void GetPath(Vector2 targetPos){
		path = pathFinder.FindPath(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), targetPos); 
	}

	IEnumerator InfiniteMoveCoroutine(){
		while(true){
			if(path.Count > 0){
				MoveObject(path.Pop());
			}

			yield return new WaitForSeconds(moveSpeed);
		}
	}

	IEnumerator MoveCoroutine(/*int _x, int _y*/){
		coroutineRunning = true;
		
		//a cute little stress test
		//slight delay for x50 runs for 90 units of distance
		//for(int i=0; i < 50; i++){
		//path = pathFinder.FindPath(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(_x, _y));
		//}
		while(path.Count > 0){
			MoveObject(path.Pop());
			yield return new WaitForSeconds(moveSpeed);
		}
		coroutineRunning = false;
	}

	void ReplanAndMove(){

		//add case where already in target grid
		//get new target coord inside the target grid
		Vector3 currentPosition = unit.transform.position;
		if(currentPosition.x >= unit.gridTarget.x*5f && currentPosition.x <= unit.gridTarget.x*5f+5f
			&& currentPosition.y >= unit.gridTarget.y*5f && currentPosition.y <= unit.gridTarget.y*5f+5f){
			if(unit.diffusing){
				unit.diffusionFail = true;
			}
			else{
				GetMovingGrid((int)(unit.gridTarget.x), (int)(unit.gridTarget.y));
			}
			
			return;
		}
		//get new path to same real world coordinate
		int layerMask0 = 1 << 8;
		int layerMask1 = 1 << 9;
		int layerMask = layerMask0 | layerMask1;
		Collider[] unitsNearby = Physics.OverlapSphere(unit.transform.position, 10f, layerMask);
		for(int i=0; i < unitsNearby.Length; i++){
			Vector3 temp = unitsNearby[i].transform.position;
			map[(int)(temp.x), (int)(temp.y)].walkable = false;
		}
		GetPath(ultTarget);
		//StartMovement();
		for(int i=0; i < unitsNearby.Length; i++){
			Vector3 temp = unitsNearby[i].transform.position;
			map[(int)(temp.x), (int)(temp.y)].walkable = true;
		}
	}

	IEnumerator ReplanningTimer(){
		replanningTimerRunning = true;
		yield return new WaitForSeconds(1.5f);
		ReplanAndMove();

		replanningTimerRunning = false;
	}

	void StartReplanningTimer(){
		if(!replanningTimerRunning){
			replanningTimerCoroutine = StartCoroutine(ReplanningTimer());
		}
	}



	void MoveObject(Vector2 target){
		
		bool crash = false;
		int layerMask0 = 1 << 8;
		int layerMask1 = 1 << 9;
		int layerMask = layerMask0 | layerMask1;
		Collider[] crashableObjects = Physics.OverlapSphere(gameObject.transform.position, 1f, layerMask);

		for(int i=0; i < crashableObjects.Length; i++){
			if(target.x == crashableObjects[i].gameObject.transform.position.x && target.y == crashableObjects[i].gameObject.transform.position.y){
				crash = true;
				break;
			}
		}
		if(crash){
			path.Push(target);
			StartReplanningTimer();
			return;
		}
		if(replanningTimerRunning){
			StopCoroutine(replanningTimerCoroutine);
			replanningTimerRunning = false;
		}
		gameObject.transform.position = new Vector3(target.x, target.y);
		
		
		
	}

	void Override(){
		if(coroutineRunning){
			StopCoroutine(moveCoroutine);
			coroutineRunning = false;
		}
	}



}
