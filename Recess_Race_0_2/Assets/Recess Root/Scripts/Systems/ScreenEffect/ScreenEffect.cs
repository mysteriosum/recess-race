using UnityEngine;
using System.Collections;

public abstract class ScreenEffect {

	public bool done;
	protected float life;

	public delegate void DoneAction();
	public event DoneAction OnDone;

	public ScreenEffect(float lifetime){
		this.life = lifetime;
	}

	public void OnGui(float time){
		if (done) return;
		life -= time;
		if (life <= 0) {
			setDone();
		}
		show();			
	}

	private void setDone(){
		done = true;
		if (OnDone != null) {
			OnDone ();		
		}

	}
	abstract public void show();
}
