using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridToRW : MonoBehaviour {



	public static Vector2 GetGridToRW(int gridCoordX, int gridCoordY, AStarMap mapHook){
		Vector2 gridStartRW = new Vector2(gridCoordX*5f, gridCoordY*5f);
		return GridToRW.GetVacantPoint((int)(gridStartRW.x), (int)(gridStartRW.y), mapHook);	
	}

	static Vector2 GetVacantPoint(int xStartRW, int yStartRW, AStarMap mapHook){
		//layerMask -> red + blue
		int layerMask = (1 << 8) | (1 << 9);
		Collider[] units = Physics.OverlapBox(new Vector3(xStartRW + 2.5f, yStartRW + 2.5f, 0f), new Vector3(2.5f, 2.5f, 0f), Quaternion.identity, layerMask);
		
		AStarNode[,] map = mapHook.map;
		for(int i=0; i < units.Length; i++){
			map[(int)(units[i].transform.position.x), (int)(units[i].transform.position.y)].walkable = false;
		}

		List<Vector2> possiblePositions = new List<Vector2>();
		for(int i=xStartRW; i < xStartRW+6; i++){
			for(int j=yStartRW; j < yStartRW+6; j++){
				if(map[i,j].walkable){
					//Debug.Log(map[i,j].pos);
					possiblePositions.Add(map[i,j].pos);
				}
			}
		}

		for(int i=0; i < units.Length; i++){
			map[(int)(units[i].transform.position.x), (int)(units[i].transform.position.y)].walkable = true;
		}

		if(possiblePositions.Count == 0){
			Debug.Log("All is lost");
			return new Vector2(-1f, -1f);
		}
		Vector2 target = possiblePositions[Random.Range(0, possiblePositions.Count)];
		return target;


	}

}
