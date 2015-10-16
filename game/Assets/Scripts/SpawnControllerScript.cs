using UnityEngine;
using System.Collections;

public class SpawnControllerScript : MonoBehaviour 
{
	public Transform trans_GamePlayMusic;
	public Transform trans_EnterTheFocus;
	public Transform trans_FocusMode;
	public Transform trans_SpeedBackUp;

	public Transform trans_PowerUpSpawn1;
	public Transform trans_PowerUpSpawn2;

	public Transform groundSpawn;
	public Transform player;

	protected SpawnScript groundSpawnScript;
	
	protected PlatformerCharacter2D playerScript;

	protected StayAwakeSound script_GamePlayMusic;
	protected StayAwakeSound script_EnterTheFocus;
	protected StayAwakeSound script_FocusMode;
	protected StayAwakeSound script_SpeedBackUp;

	protected PowerUpSpawn script_PowerUpSpawn1;
	protected PowerUpSpawn script_PowerUpSpawn2;

	protected float powerDropStart;
	protected float sloMoStart;

	protected bool gamePaused = false;

	protected float powerDropDuration = 3.0f;
	protected float sloMoDuration = 4.5f;

	protected bool powerDropActive = false;
	protected bool sloMoActive = false;

	protected float spawnDelay = 0.79f;
	protected float sloMoSpeed = 2.0f;
	protected float addFocusModeDelay = 20.0f;

	protected float playerGravityScale;
	protected float playerMass;
	protected float playerAngularDrag;

	protected bool spawnStarted = false;

	void Awake()
	{
		Time.timeScale = 1;

		trans_GamePlayMusic = Instantiate(trans_GamePlayMusic) as Transform;
		script_GamePlayMusic = trans_GamePlayMusic.gameObject.GetComponent<StayAwakeSound>();	

		trans_EnterTheFocus = Instantiate(trans_EnterTheFocus) as Transform;
		script_EnterTheFocus = trans_EnterTheFocus.gameObject.GetComponent<StayAwakeSound>();	

		trans_FocusMode = Instantiate(trans_FocusMode) as Transform;
		script_FocusMode = trans_FocusMode.gameObject.GetComponent<StayAwakeSound>();	

		trans_SpeedBackUp = Instantiate(trans_SpeedBackUp) as Transform;
		script_SpeedBackUp = trans_SpeedBackUp.gameObject.GetComponent<StayAwakeSound>();	

		trans_PowerUpSpawn1 = Instantiate(trans_PowerUpSpawn1) as Transform;
		script_PowerUpSpawn1 = trans_PowerUpSpawn1.gameObject.GetComponent<PowerUpSpawn>();

		trans_PowerUpSpawn2 = Instantiate(trans_PowerUpSpawn2) as Transform;
		script_PowerUpSpawn2 = trans_PowerUpSpawn2.gameObject.GetComponent<PowerUpSpawn>();

		groundSpawnScript = groundSpawn.gameObject.GetComponent<SpawnScript>();	

		playerScript = player.gameObject.GetComponent<PlatformerCharacter2D>();
	}

	// Use this for initialization
	void Start() 
	{
		GameControl.control.Load();

		GameControl.control.focusModeCount = 3;
	
		GameControl.control.gamePlayMusicMS = script_GamePlayMusic.GetSoundTime();
		script_GamePlayMusic.Play(true);
	}

	void Update()
	{
		if(!spawnStarted)
		{
			SpawnRoom();
			AddFocusMode();

			spawnStarted = true;
		}

		if (Input.GetKeyDown (KeyCode.W) && !powerDropActive && (GameControl.control.powerUpCount >= 10))
		{
			Component[] GroundPieces = FindObjectsOfType(typeof(ScrollingObject)) as Component[];

			foreach (Component ground in GroundPieces) 
			{
				foreach (Transform child in ground.gameObject.transform)
				{
					if(child.gameObject.GetComponent<Collider2D>() != null)
						child.gameObject.GetComponent<Collider2D>().enabled = false;
					
					foreach (Transform groundChild in child.gameObject.transform)
					{
						if(groundChild.gameObject.GetComponent<Collider2D>() != null)
							groundChild.gameObject.GetComponent<Collider2D>().enabled = false;
					}
				}
			}

			GameControl.control.powerUpCount = 0;

			groundSpawnScript.SetUsingColliders(false);

			powerDropStart =  Time.time;
			powerDropActive = true;
		}

		if (Input.GetKeyDown (KeyCode.S) && !sloMoActive && (GameControl.control.focusModeCount > 0))
		{
			playerScript.DoubleSpeed();

			GameControl.control.gamePlayMusicMS = script_GamePlayMusic.GetSoundTime();
			script_GamePlayMusic.Stop();

			script_EnterTheFocus.Play(false);
			script_FocusMode.Play(false);

			playerGravityScale = player.gameObject.GetComponent<Rigidbody2D>().gravityScale;
			playerMass = player.gameObject.GetComponent<Rigidbody2D>().mass;
			playerAngularDrag = player.gameObject.GetComponent<Rigidbody2D>().angularDrag;

			player.gameObject.GetComponent<Rigidbody2D>().gravityScale *= 2;
			player.gameObject.GetComponent<Rigidbody2D>().mass *= 2;
			player.gameObject.GetComponent<Rigidbody2D>().angularDrag *= 2;

			Time.timeScale = 0.5f;

			GameControl.control.focusModeCount--;

			Invoke("PlaySpeedBackUp", 3.5f);

			sloMoStart =  Time.time;
			sloMoActive = true;
		}

		if(powerDropActive && Time.time > (powerDropStart + powerDropDuration))
		{
			powerDropActive = false;
			groundSpawnScript.SetUsingColliders(true);
		}

		if(sloMoActive && Time.time > (sloMoStart + sloMoDuration))
		{
			sloMoActive = false;

			Time.timeScale = 1;

			playerScript.ResetSpeed();
			player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
			player.gameObject.GetComponent<Rigidbody2D>().mass = 1;
			player.gameObject.GetComponent<Rigidbody2D>().angularDrag = 0.5f;
		}

		if(Input.GetKeyUp (KeyCode.P))
		{
			gamePaused = true;
			Time.timeScale = 0;
		}
	}

	public void PlaySpeedBackUp()
	{
		script_SpeedBackUp.Play(false);
		Invoke("ResumeGamePlayMusic", 1.25f);
	}

	public void ResumeGamePlayMusic()
	{
		script_GamePlayMusic.SetSoundTime(GameControl.control.gamePlayMusicMS);
		script_GamePlayMusic.Play(true);
	}

	void SpawnRoom() 
	{
		if(Random.Range(0, 3) == 0)
		{
			if(Random.Range(0, 2) == 0)
				script_PowerUpSpawn1.Spawn();
			else
				script_PowerUpSpawn2.Spawn();
		}

		groundSpawnScript.Spawn();
		Invoke("SpawnRoom", spawnDelay);
	}

	void AddFocusMode()
	{
		if(GameControl.control.focusModeCount < 3)
			GameControl.control.focusModeCount++;

		Invoke("AddFocusMode", addFocusModeDelay);
	}

	void OnGUI()
	{
		if(gamePaused)
		{
			if(GUI.Button(new Rect(380,275,140,80), "Keep Playing")) 
			{
				gamePaused = false;
				Time.timeScale = 1;
			}
		}
	}
}
