using UnityEngine;
using System.Collections;

public class RankIndicator : MonoBehaviour {
	
	public Transform[] bullies;
	public Transform fitz;
	
	private Color[] colors = new Color[] {Color.cyan, Color.green, Color.yellow, Color.red};
	private string[] strings = new string[] {"1st", "2nd", "3rd", "4th"};
	private int[] sizes = new int[] {200, 185, 160, 150};
	
	private TextMesh mesh;
	
	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		int place = 0;
		foreach (Transform b in bullies){
			if (b.position.y > fitz.position.y)
				place ++;
		}
		
		mesh.text = strings[place];
		mesh.renderer.material.color = colors[place];
		mesh.fontSize = sizes[place];
	}
	
	
}
