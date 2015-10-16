using UnityEngine;
using System.Collections;

public class PowerDropCollectable : ScrollingObject 
{
	public Transform trans_BlipSound;
	
	protected StayAwakeSound script_BlipSound;
	
	void Awake()
	{
		trans_BlipSound = Instantiate(trans_BlipSound) as Transform;
		script_BlipSound = trans_BlipSound.gameObject.GetComponent<StayAwakeSound>();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Player")
		{
			script_BlipSound.Play(false);

			GameControl.control.powerUpCount++;

			this.gameObject.GetComponent<Renderer>().enabled = false;
			//Destroy(this.gameObject);
		}
	}

	void OnDestroy()
	{
		if(trans_BlipSound != null)
		{
			Destroy(trans_BlipSound.gameObject);
		}
	}
}
