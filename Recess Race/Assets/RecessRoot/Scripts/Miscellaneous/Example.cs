using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {
	
	public int howManyPeople = 0;
	public string[] namesOfPeople;
	public bool peopleInRooom;
	
	[System.Serializable]
	public class MoreInfo{
		public int pogs;
		public string pogName;
		public bool integer;
	}
	
	public MoreInfo[] infos = new MoreInfo[5] {new MoreInfo(), new MoreInfo(), new MoreInfo(), new MoreInfo(), new MoreInfo()};
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
