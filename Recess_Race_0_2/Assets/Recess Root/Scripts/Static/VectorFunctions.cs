using UnityEngine;
using System.Collections;

static class VectorFunctions {

	/// <summary>
	/// Convert the specified angle (0 to 180 or -1 to -181) to a positive one (0 to 359).
	/// </summary>
	/// <param name='angle'>
	/// The angle to convert.
	/// </param>
	static public float Convert360(float angle){		//convert an angle which is normally 0-360 into one that's 0-180 or -180 - -1
		return angle < 180 ? angle : (angle - 2 * (angle - 180)) * -1;
	}
	/// <summary>
	/// Gets the slope of a vector.
	/// </summary>
	/// <returns>
	/// The direction in radians of the vector in question.
	/// </returns>
	/// <param name='vector'>
	/// Vector.
	/// </param>
	static public float PointDirection(Vector2 vector){
		 float angle = Mathf.Acos(vector.x / vector.magnitude) * Mathf.Rad2Deg;
			
			if (vector.y < 0){
				angle *= -1;
			}
		return angle;
	}
	
	static public Vector3 ConvertLookDirection(Vector3 convertee){
		return new Vector3(convertee.z, convertee.y, convertee.x);
	}
	/// <summary>
	/// Finds the closest of a specified tag.
	/// </summary>
	/// <returns>
	/// The closest Transform of the tag appropriate tag to [closestTo].
	/// </returns>
	/// <param name='closestTo'>
	/// What point do I want to compare the objects to?
	/// </param>
	/// <param name='tag'>
	/// The tag.
	/// </param>
	/// <param name='maxDistance'>
	/// The furthest distance I'll check
	/// </param>
	static public Transform FindClosestOfTag(Vector3 closestTo, string tag, int maxDistance){
		GameObject[] tagged = GameObject.FindGameObjectsWithTag(tag);
		float closestDist = maxDistance;
		Transform closestHere = null;
		foreach (GameObject n in tagged){
			float tempDist = (closestTo - n.transform.position).magnitude;		//This was cast as (Vector2), but I don't think it's necessary. Maybe it will be eventually?
			
			if (tempDist < closestDist){
				closestDist = tempDist;
				closestHere = n.transform;
			}
		}
		
		return closestHere;
		
	}
	/// <summary>
	/// Bounce the velocityBefore according to a normal.
	/// </summary>
	/// <param name='velocityBefore'>
	/// Velocity before.
	/// </param>
	/// <param name='normal'>
	/// Normal.
	/// </param>
	static public Vector2 Bounce (Vector2 velocityBefore, Vector2 normal) {
		Vector2 modified = new Vector2(normal.y, normal.x);
		return (2 * Vector2.Dot(velocityBefore, modified) * modified - velocityBefore);
	}
	
	static public Vector2 Bounce (Vector2 velocityBefore, Vector3 normal) {
		return Bounce (velocityBefore, (Vector2) normal);
	}
	
	/// <summary>
	/// Create a LookRotation based on a single direction (where you want the sprite facing) for 2D.
	/// </summary>
	/// <returns>
	/// The Quaternion to set your t.rotation to
	/// </returns>
	/// <param name='direction'>
	/// Where you want to face
	/// </param>
	static public Quaternion Look2D (Vector2 direction){
		float cs = Mathf.Cos(Mathf.Deg2Rad * 90);
		float sn = Mathf.Sin(Mathf.Deg2Rad * 90);
		
		Vector3 up = new Vector3(direction.x * cs - direction.y * sn, direction.x * sn + direction.y * cs);
			
		Quaternion newLook = Quaternion.LookRotation(new Vector3(0, 0, 1), up);
		
		return newLook;
	}
	
	static public Quaternion Look2D(Vector3 direction){
		return Look2D(new Vector2(direction.x, direction.y));
	}
}
