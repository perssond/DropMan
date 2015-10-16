using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour 
{
	public Transform trans_GameOverSound;
	
	protected StayAwakeSound script_GameOverSound;
	
	public bool gameOver = false;

	void Awake()
	{
		gameOver = false;
		trans_GameOverSound = Instantiate(trans_GameOverSound) as Transform;
		script_GameOverSound = trans_GameOverSound.gameObject.GetComponent<StayAwakeSound>();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Player" && !gameOver)
		{
			script_GameOverSound.Play(false);
			gameOver = true;

			Time.timeScale = 0;
		}
	}
}
