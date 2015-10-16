using UnityEngine;
using System.Collections;

public class MainScreenScript : MonoBehaviour 
{
	public Transform trans_GameIntroMusic;

	protected StayAwakeSound script_GameIntroMusic;

	private bool playerNeedsName = false;
	private string newNameString = "Enter Name";

	// Use this for initialization
	void Start () 
	{
		GameControl.control.Load();

		if (GameControl.control.playerName == "")
		{
			playerNeedsName = true;
		}	

		trans_GameIntroMusic = Instantiate(trans_GameIntroMusic) as Transform;
		script_GameIntroMusic = trans_GameIntroMusic.gameObject.GetComponent<StayAwakeSound>();

		script_GameIntroMusic.SetSoundTime(GameControl.control.gameIntroMusicMS);
		script_GameIntroMusic.Play(true);
	}

    // writing some new code
	void OnGUI()
	{
		if(!playerNeedsName)
		{
			if(GUI.Button(new Rect((Screen.width / 2) - 70, (Screen.height / 2) - 85, 140, 80), "Start Game")) 
			{
				Application.LoadLevel("DropMan");
			}

			if(GUI.Button(new Rect((Screen.width / 2) - 70, (Screen.height / 2), 140, 80), "High Scores")) 
			{
				GameControl.control.gameIntroMusicMS = script_GameIntroMusic.GetSoundTime();
				GameControl.control.Save();
				Application.LoadLevel("HighScores");
			}

			GUI.Label(new Rect(10, Screen.height - 20, 100, 30), "Player: " + GameControl.control.playerName);
		}
		else
		{
			newNameString = GUI.TextField(new Rect(310, 255, 100, 30), newNameString, 25);

			if(GUI.Button(new Rect(420,248,80,40), "Submit")) 
			{
				GameControl.control.playerName = newNameString;
				GameControl.control.Save();

				playerNeedsName = false;
			}
		}
	}
}
