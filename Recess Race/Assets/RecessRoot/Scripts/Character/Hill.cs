using UnityEngine;
using System.Collections;

public class Hill : Profile {
	
	
	
	void Start () {
		base.Start();
		Chars.hill = this;
	}
	
	
	void Update () {
		base.Update();
	}
}
