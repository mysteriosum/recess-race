using UnityEngine;
using System.Collections;

public class CollisionGenerator : MonoBehaviour {
	BoxCollider oCol;
	
	GameObject topCol;
	GameObject botCol;
	GameObject leftCol;
	GameObject rightCol;
	
	Transform t;
	
	float margin = 4;
	
	float minMarginDividor = 4;
	float normColSize;
	
	// Use this for initialization
	void Start () {
		oCol = GetComponent<BoxCollider>();
		t = transform;
		
		normColSize = oCol.size.x;
		
		float hori = oCol.size.x * t.localScale.x;
		float vert = oCol.size.y * t.localScale.y;
		
		float hMargin = Mathf.Min(margin, hori / minMarginDividor);
		float vMargin = Mathf.Min(margin, vert / minMarginDividor);
		
		topCol = Instantiate(Resources.Load("colliders/top"), t.position + new Vector3(hMargin, vert - vMargin, 0), t.rotation) as GameObject;
		botCol = Instantiate(Resources.Load("colliders/bot"), t.position + new Vector3(hMargin, 0, 0), t.rotation) as GameObject;
		rightCol = Instantiate(Resources.Load("colliders/right"), t.position + new Vector3(hori - hMargin, 0, 0), t.rotation) as GameObject;
		leftCol = Instantiate(Resources.Load("colliders/left"), t.position, t.rotation) as GameObject;
		
		
		topCol.transform.localScale = new Vector3(t.localScale.x - hMargin*2/normColSize, vMargin/normColSize, t.localScale.z);
		botCol.transform.localScale = new Vector3(t.localScale.x - hMargin*2/normColSize, vMargin/normColSize, t.localScale.z);
		rightCol.transform.localScale = new Vector3(hMargin/normColSize, t.localScale.y, t.localScale.z);
		leftCol.transform.localScale = new Vector3(hMargin/normColSize, t.localScale.y, t.localScale.z);
		
		Destroy(gameObject);
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
