using UnityEngine;
using System.Collections;

public class Player : Character {
	/*
	
	
	public class Controller {
		public bool getRun;
		public bool getRunDown;
		public bool getRunUp;
		
		public bool getJump;
		public bool getJumpDown;
		public bool getJumpUp;
		
		public bool getL;
		public bool getLUp;
		public bool getLDown;
		
		public bool getR;
		public bool getRUp;
		public bool getRDown;
		
		public bool getD;
		public bool getDDown;
		public bool getDUp;
		
		public bool getU;
		public bool getUUp;
		public bool getUDown;
		
		public bool doubleTap;
		public bool aboutFace;
	
		public bool locked;
		
		private float hAxisLast = 0.0f;
		private float vAxisLast = 0.0f;
		private float getLLastTime = 0f;
		private float getRLastTime = 0f;
		private float doubleTapTime = 0.2f;
		
		private Player parent;
		
		public Controller (Player parent){
			this.parent = parent;
		}
		
		public void GetInputs(){
			if (locked) return;
			
			getRun = Input.GetButton("Run");
			getRunDown = Input.GetButtonDown("Run");
			getRunUp = Input.GetButtonUp("Run");
			
			if (getRunDown && parent.runDown != null)
				parent.runDown();
			
			if (getRunUp && parent.runUp != null)
				parent.runUp();
			
			if (getRun && parent.run != null)
				parent.run();
			
			getJump = Input.GetButton("Jump");
			getJumpDown = Input.GetButtonDown("Jump");
			getJumpUp = Input.GetButtonUp("Jump");
			
			if (getJumpDown && parent.jumpDown != null)
				parent.jumpDown();
			
			if (getJumpUp && parent.jumpUp != null)
				parent.jumpUp();
			
			float hAxis = Input.GetAxis("Horizontal");
			
			if (hAxis != 0 && parent.direction != null){
				parent.direction(hAxis);
			}
			
			getL = hAxis < -0.3f;
			getR = hAxis > 0.3f;
			
			getLDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getL;
			getRDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getR;
			
			getLUp = hAxisLast < -0.3f && !getL;
			getRUp = hAxisLast > 0.3f && !getR;
			
			if ((getLUp || getRUp) && parent.directionUp != null && !getR && !getL){
				parent.directionUp();
			}
			
			if ((getLDown || getRDown) && parent.directionDown != null){
				parent.directionDown();
			}
			
			if (getLDown){						//check for doubleTap
				if (Time.time - getLLastTime < doubleTapTime){
					doubleTap = true;
					if (parent.doubleTap != null)
						parent.doubleTap();
				}
				getLLastTime = Time.time;
			}
			
			if (getRDown){
				if (Time.time - getRLastTime < doubleTapTime){
					doubleTap = true;
					if (parent.doubleTap != null)
						parent.doubleTap();
				}
				getRLastTime = Time.time;
			}
			hAxisLast = hAxis;
			
			float vAxis = Input.GetAxis ("Vertical");
			getD = vAxis < -0.3f;
			getU = vAxis > 0.3f;
			
			getDDown = (vAxisLast < 0.3f && vAxisLast > -0.3f) && getD;
			getUDown = (vAxisLast < 0.3f && vAxisLast > -0.3f) && getU;
			
			if (getDDown && parent.downDown != null){
				parent.downDown();
			}
			
			getDUp = vAxisLast < -0.3f && !getD;
			getUUp = vAxisLast > 0.3f && !getU;
			
			if (getDUp && parent.downUp != null){
				parent.downUp();
			}
			
			vAxisLast = vAxis;
		}
		
	}
	
	Controller controller;
	
	// Use this for initialization

	protected override void Start ()
	{
		base.Start ();
		controller = new Controller(this);
		
	}
	
	// Update is called once per frame
	void Update () {
		controller.GetInputs();
	}
	*/
}
 