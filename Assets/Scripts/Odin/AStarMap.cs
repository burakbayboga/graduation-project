using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap : MonoBehaviour {

	public int mapSize;
	public AStarNode[,] map;

	void Start(){
		map = new AStarNode[mapSize, mapSize];
		for(int i=0; i < mapSize; i++){
			for(int j=0; j < mapSize; j++){
				AStarNode newNode = new AStarNode();
				newNode.pos = new Vector2(i,j);
				map[i,j] = newNode;
			}
		}

		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		Vector3 tempPosition;
		for(int i=0; i < obstacles.Length; i++){
			tempPosition = obstacles[i].transform.position;
			map[(int)tempPosition.x, (int)tempPosition.y].walkable = false;
		}
		
	}
}
