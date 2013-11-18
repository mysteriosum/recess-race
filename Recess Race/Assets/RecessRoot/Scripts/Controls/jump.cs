using UnityEngine;
using System.Collections;

public class jump : MonoBehaviour {
	
	
	Rigidbody rigsauce;
	GameObject d;
	
	//velocity values;
	public float initpush = 80.9f;
	public float secondpush = 57.79f;
	public float lastpush = 14.8f;
	public float fallForce = 90.12f;
	public float fallForceSecond = 11.3f;
	public float maxFall = 30.0f;
	
	
	public float grav = 5.33f;//custom gravity
	
	public int jumptimer=10;//how long holding a jump will do anything
	public int jumpcounter;
	public int jumpfraction = 15;//divider of jump for alt forces
	
	public bool isJumping;//states, should be from other class
	public bool onGround;
	public bool firstJump;
	
	
	public int numJumps = 1;
	public int availableJumps;
	public int presslength=0;
	public int falltimer = 0;
	
	public Vector3 veloc;
	public float mostFall = 0.0f;
	public int timeout = 5;
	public int currentTime=0;
	// Use this for initialization
	protected void Start () {
		d= gameObject;
		rigsauce=d.rigidbody;
		Physics.gravity = new Vector3(0,-grav,0);
		jumpcounter=0;
		firstJump=true;
		availableJumps = numJumps;
		
	}
	
	protected virtual void Setup (){
	
	}

	
	// Update is called once per frame
	void Update () {
		
		if(onGround&&currentTime<timeout)
			currentTime++;
		
		if(Input.GetKeyDown(KeyCode.Space)&&availableJumps!=0){
			if(/*onGround&&*/firstJump){//&&currentTime==timeout){
				rigsauce.velocity= new Vector3(rigsauce.velocity.x,initpush,0);
				onGround=false;
				isJumping=true;
				currentTime=0;
				jumpcounter=0;
				availableJumps--;
				firstJump=false;
			}
		}
		
		if(Input.GetKey(KeyCode.Space)){//holding jump
			presslength++;
			if(isJumping){
				if(jumpcounter<(jumptimer/jumpfraction)&&jumpcounter!=0){
					rigsauce.velocity+= new Vector3(0,secondpush,0);
					jumpcounter++;
				}
				else{
					rigsauce.velocity+= new Vector3(0,lastpush,0);
					jumpcounter++;
				}
			}
			//play floating jump up
		}
		if(!isJumping&&!onGround){//in air, falling
			rigsauce.velocity+= new Vector3(0,-(fallForceSecond),0);
			firstJump=true;
		} 
		if((jumpcounter>=jumptimer&&isJumping)||(Input.GetKeyUp(KeyCode.Space)&&isJumping)){//jump has ended
			isJumping=false;
			
				rigsauce.velocity+= new Vector3(0,-(fallForce),0);

			//play jumpfall
		}
	/*	
			
		#region multiJump
		
			if(Input.GetKeyDown(KeyCode.Space)){
			if(onGround&&jumpcounter==0){//&&currentTime==timeout){
				rigsauce.velocity+= new Vector3(0,initpush,0);
				onGround=false;
				isJumping=true;
				currentTime=0;
			}
		}
		
		if(Input.GetKey(KeyCode.Space)){//holding jump
			presslength++;
			if(isJumping){
				if(jumpcounter<(jumptimer/jumpfraction)&&jumpcounter!=0){
					rigsauce.velocity+= new Vector3(0,secondpush,0);
					jumpcounter++;
				}
				else{
					rigsauce.velocity+= new Vector3(0,lastpush,0);
					jumpcounter++;
				}
			}
			//play floating jump up
		}
		if(!isJumping&&!onGround){//in air, falling
			rigsauce.velocity+= new Vector3(0,-(fallForceSecond),0);
		} 
		if((jumpcounter>=jumptimer&&isJumping)||(Input.GetKeyUp(KeyCode.Space)&&isJumping)){//jump has ended
			isJumping=false;
			
				rigsauce.velocity+= new Vector3(0,-(fallForce),0);

			//play jumpfall
		}
		
		#endregion
	
		*/
		
		
		#region ray
		       //------------------------------------CHECK GROUNDED--------------------------------
                if (!onGround&&!isJumping){
                       
                        RaycastHit hitInformation;
                        Ray midGroundedRay = new Ray(collider.bounds.center, Vector3.down);
                        float rayDistance = collider.bounds.size.y/2 + Mathf.Abs (rigsauce.velocity.y * Time.deltaTime);
                       
                        bool raycast = Physics.Raycast(midGroundedRay, out hitInformation, rayDistance);
                       
                        if (raycast){
                            this.onGround = true;
                            transform.Translate (new Vector3(0, -(hitInformation.distance - (collider.bounds.size.y)/2), 0));
                    		rigsauce.velocity = new Vector3(rigsauce.velocity.x,0,0);
                    		jumpcounter=0;
							availableJumps=numJumps;
                        }
						else{
							//onGround=false;
							//isJumping=false;
						}
                }
		
				if(onGround&&!isJumping){
					RaycastHit hitRight;
                    Ray midGroundedRayRight = new Ray(new Vector3(collider.bounds.center.x+(collider.bounds.size.x/2),collider.bounds.center.y,0), Vector3.down);
                    float rayDistanceRight = collider.bounds.size.y/2 + Mathf.Abs (rigsauce.velocity.y * Time.deltaTime);
                    bool raycastRight = Physics.Raycast(midGroundedRayRight, out hitRight, rayDistanceRight);
			
					RaycastHit hitLeft;
                    Ray midGroundedRayLeft = new Ray(new Vector3(collider.bounds.center.x-(collider.bounds.size.x/2),collider.bounds.center.y,0), Vector3.down);
                    float rayDistanceLeft = collider.bounds.size.y/2 + Mathf.Abs (rigsauce.velocity.y * Time.deltaTime);
                    bool raycastLeft = Physics.Raycast(midGroundedRayLeft, out hitLeft, rayDistanceLeft);
			
			
                    if (!raycastRight&&!raycastLeft){
				    		Debug.Log ("DERP");
							onGround=false;
							isJumping=false;
                    }
				
					availableJumps=numJumps;
					
				}
               
                //----------------------------------END GROUNDED CHECK------------------------------
		#endregion
		
		if(Input.GetKey(KeyCode.D))
			rigsauce.AddForce(new Vector3(500.0f,0,0));
		
		if(Input.GetKey(KeyCode.A))
			rigsauce.AddForce(new Vector3(-500.0f,0,0));
		
		if(Input.GetKey(KeyCode.J)){
			d.transform.position=new Vector3(-28.0f,44.0f,0.0f);
			Debug.Log ("reset");	
		}
		
		
		veloc=rigsauce.velocity;
		if(veloc.y<mostFall)
			mostFall=veloc.y;
		
	}
	

	
	void OnCollisionEnter(Collision collision)
	{
		//iftag=cieling or bully
		//this.isJumping=false;
		//iftag=ground
   		this.onGround=true;
		rigsauce.velocity = new Vector3(rigsauce.velocity.x,0,0);
		jumpcounter=0;
		//play land
	}  
	
	       void OnDrawGizmos(){
                Gizmos.color = Color.red;
                Gizmos.DrawLine (collider.bounds.center, collider.bounds.center - new Vector3(0, collider.bounds.size.y + maxFall/60 /*stand-in for Time.deltaTime*/, 0));
        }
	   
}
