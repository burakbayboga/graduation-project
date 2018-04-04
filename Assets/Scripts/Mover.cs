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
	

	AStarNode[,] map;
	Stack<Vector2> path;
	bool speedCooldown;


	
	void Start(){
		unit = GetComponent<GCS>();
		moveSpeed = unit.moveSpeed;
		coroutineRunning = false;
		pathFinder = new AStar();
		map = unit.odin.GetComponent<AStarMap>().map;
		pathFinder.map = map;
		speedCooldown = false;
	

		
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

	public void GetMoving(int _x, int _y){
		Override();
		moveCoroutine = StartCoroutine(MoveCoroutine(_x, _y));
	}

	IEnumerator MoveCoroutine(int _x, int _y){
		coroutineRunning = true;
		path = new Stack<Vector2>();
		//a cute little stress test
		//slight delay for x50 runs for 90 units of distance
		//for(int i=0; i < 50; i++){
			path = pathFinder.FindPath(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(_x, _y));
		//}
		while(path.Count > 0){
			MoveObject(path.Pop());
			yield return new WaitForSeconds(moveSpeed);
		}
		coroutineRunning = false;
	}

	IEnumerator ReplanningTimer(){
		replanningTimerRunning = true;
		yield return new WaitForSeconds(3f);


		replanningTimerRunning = false;
	}

	void StartReplanningTimer(){
		if(!replanningTimerRunning){
			StartCoroutine(ReplanningTimer());
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
		gameObject.transform.position = new Vector3(target.x, target.y);
		
		
		
	}

	void Override(){
		if(coroutineRunning){
			StopCoroutine(moveCoroutine);
			coroutineRunning = false;
		}
	}



}
