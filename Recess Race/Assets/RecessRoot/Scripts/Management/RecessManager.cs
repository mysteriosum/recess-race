using UnityEngine;
using System.Collections;

public class RecessManager : MonoBehaviour {
	
	static RecessManager instance;
	
	public int levelOffset = 200;
	
	public int loadLevelMin = 0;
	public int loadLevelMax = 2;
	
	public static RecessManager Instance{
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(RecessManager)) as RecessManager;
			
			return instance;
		}
	}
	
	public int loadingLevel = 0;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		
		for (loadingLevel = 0; loadingLevel < 10; loadingLevel ++){
			Application.LoadLevelAdditive("floor_" + Random.Range(loadLevelMax, loadLevelMin).ToString());
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
