using UnityEngine;
using System;
using System.Collections;

public class Plateform : IComparable{

	private static int nextId = 0;
	public int uniqueId;
	public Transform transform;
	public Bounds bound;

	public Plateform(Transform transform, Bounds bound){
		this.uniqueId = nextId++;
		this.transform = transform;
		this.bound = bound;
	}

	public int CompareTo(object obj) {
		if (obj == null)
			return 1;
		Plateform otherP = (Plateform)obj;

		return this.uniqueId - otherP.uniqueId;
	}

	public bool isUnder(Plateform p2){
		Vector3 v1 = this.transform.position; 
		Vector3 v2 = p2.transform.position;
		
		return (v2.y > v1.y) || (v1.y == v2.y && (v2.x < v1.x));
	}

	public bool isSame(Plateform p2){
		return this.bound.Equals (p2.bound);
	}
}
