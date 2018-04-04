using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarNode {

	public float gCost;
	public float hCost;
	public bool isClosed;
	public bool isOpen;
	public bool walkable;
	public Vector2 pos;
	public AStarNode parent;

	public AStarNode(){
		gCost = 0;
		hCost = 0;
		isClosed = false;
		isOpen = false;
		walkable = true;
		parent = null;
	}

	public float FCost(){
		return gCost + hCost;
	}

	public void CalcGCost(){
		gCost = 0;
		if(parent != null){
			if(pos.x == parent.pos.x || pos.y == parent.pos.y){
				gCost = parent.gCost + 1f;
			}
			else{
				gCost = parent.gCost + 1.4f;
			}
		}
	}

	public void CalcHCost(Vector2 targetPos){
		hCost = (pos - targetPos).magnitude;
		//hCost = (int)(Math.Abs(pos.x - targetPos.x) + Math.Abs(pos.y - targetPos.y));
	}
	
}
