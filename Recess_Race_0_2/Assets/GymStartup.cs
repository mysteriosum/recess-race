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

	private bool once = false;

	void Update () {
		if (!once) {
			once = true;
			ScreenEffect e = new FadeOut(2,new Color(1,0.8f,0));
			ScreenEffectSystem.AddScreenEffect(e);
		}
	}


}
