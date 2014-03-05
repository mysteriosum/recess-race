using UnityEngine;
using System.Collections;

public class GymStartup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var agents = GameObject.FindObjectsOfType<Agent>();
        foreach (var agent in agents) {
            agent.setActivated();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
