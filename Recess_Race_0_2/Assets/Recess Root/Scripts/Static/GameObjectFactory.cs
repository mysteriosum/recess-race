using UnityEngine;
using System.Collections;

public class GameObjectFactory  {

	public static GameObject createGameObject(string name, GameObject parent = null){
		GameObject newObject = new GameObject();
		newObject.name = name;
		if (parent != null) {
			newObject.transform.parent = parent.transform;		
		}
		return newObject;
	}

	public static GameObject createCopyGameObject(GameObject original , string name, GameObject parent = null){
		GameObject newObject = (GameObject)GameObject.Instantiate (original);
		newObject.name = name;
		if (parent != null) {
			newObject.transform.parent = parent.transform;		
		}

		return newObject;
	}
}
