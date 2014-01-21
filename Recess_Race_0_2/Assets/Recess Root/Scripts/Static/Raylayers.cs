using UnityEngine;
using System.Collections;

public class Raylayers{
	public static readonly int onlyCollisions;
	public static readonly int upRay;
	public static readonly int downRay;
	
	
	static Raylayers(){
		onlyCollisions = 1 << LayerMask.NameToLayer("normalCollisions");
		upRay = 1 << LayerMask.NameToLayer("normalCollisions") | 1 << LayerMask.NameToLayer("softTop");
		downRay = 1 << LayerMask.NameToLayer("normalCollisions") | 1 << LayerMask.NameToLayer("softBottom");
	}
}