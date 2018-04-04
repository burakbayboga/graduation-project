using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {

	Stack<Vector2> path;
	List<AStarNode> openList;
	List<Vector2> alteredNodes;

	public AStarNode[,] map;

	public Stack<Vector2> FindPath(Vector2 startPos, Vector2 targetPos){
		alteredNodes = new List<Vector2>();
		openList = new List<AStarNode>();
		path = new Stack<Vector2>();
		int mapSize = map.GetLength(0);
		
		AStarNode startNode = map[(int)startPos.x, (int)startPos.y];
		openList.Add(startNode);
		alteredNodes.Add(startPos);
		startNode.parent = null;
		AStarNode targetNode = map[(int)targetPos.x, (int)targetPos.y];

		AStarNode temp;
		AStarNode current;

		while(openList.Count > 0){
			current = GetLowestFNode();
			current.isClosed = true;

			if(current == targetNode){
				break;
			}

			//UP
			if(current.pos.y+1 < mapSize){
				temp = map[(int)current.pos.x, (int)current.pos.y+1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//UP RIGHT
			if(current.pos.x+1 < mapSize && current.pos.y+1 < mapSize){
				temp = map[(int)current.pos.x+1, (int)current.pos.y+1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//RIGHT
			if(current.pos.x+1 < mapSize){
				temp = map[(int)current.pos.x+1, (int)current.pos.y];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//DOWN RIGHT
			if(current.pos.x+1 < mapSize && current.pos.y-1 >= 0){
				temp = map[(int)current.pos.x+1, (int)current.pos.y-1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//DOWN
			if(current.pos.y-1 >= 0){
				temp = map[(int)current.pos.x, (int)current.pos.y-1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//DOWN LEFT
			if(current.pos.x-1 >= 0 && current.pos.y-1 >= 0){
				temp = map[(int)current.pos.x-1, (int)current.pos.y-1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//LEFT
			if(current.pos.x-1 >= 0){
				temp = map[(int)current.pos.x-1, (int)current.pos.y];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}

			//UP LEFT
			if(current.pos.x-1 >= 0 && current.pos.y+1 < mapSize){
				temp = map[(int)current.pos.x-1, (int)current.pos.y+1];
				if(temp.walkable && !temp.isClosed){
					ProcessNode(temp, current, targetPos);
				}
			}
		}

		FormPath(targetNode);

		for(int i=0; i < alteredNodes.Count; i++){
			map[(int)(alteredNodes[i].x), (int)(alteredNodes[i].y)].isOpen = false;
			map[(int)(alteredNodes[i].x), (int)(alteredNodes[i].y)].isClosed = false;
		}

		return path;

	}

	void FormPath(AStarNode traverse){
		while(traverse.parent != null){
			path.Push(traverse.pos);
			traverse = traverse.parent;
		}
	}

	void ProcessNode(AStarNode temp, AStarNode current, Vector2 targetPos){
		if(!temp.isOpen){
			temp.parent = current;
			temp.CalcGCost();
			temp.CalcHCost(targetPos);
			openList.Add(temp);
			alteredNodes.Add(temp.pos);
			temp.isOpen = true;
		}
		else{
			//ALREADY IN OPEN LIST
			if(temp.gCost > current.gCost+1f){
				temp.parent = current;
				temp.CalcGCost();
			}
		}
	}

	AStarNode GetLowestFNode(){
		float lowestF = float.MaxValue;
		int lowestFIndex = 0;
		for(int i=0; i < openList.Count; i++){
			if(openList[i].FCost() < lowestF){
				lowestF = openList[i].FCost();
				//lowestFIndex = i;
			}
		}
		AStarNode temp = openList[lowestFIndex];
		openList.Remove(temp);
		return temp;
	}
}
