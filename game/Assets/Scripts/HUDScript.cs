using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour 
{
	public float playerScore = 0;
	public Transform trans_Restarter;

	protected SubmitHighScore script_SubmitHighScore;
	protected Restarter script_Restarter;

	protected bool waitingForScoreToPost = false;

	void Awake()
	{
		script_SubmitHighScore = this.gameObject.GetComponent<SubmitHighScore>();
		script_Restarter = trans_Restarter.gameObject.GetComponent<Restarter>();
	}

	// Update is called once per frame
	void Update () 
	{
		playerScore += Time.deltaTime;
	}

	public void IncreaseScore(int amount)
	{
		playerScore += amount;
	}

	void OnGUI()
	{
		if(GameControl.control.powerUpCount >= 10)
			GUI.Label(new Rect((Screen.width) - 85, 10, 150, 60), "Power Drop" + "\n" + "    Active");
		else
			GUI.Label(new Rect((Screen.width) - 85, 10, 150, 60), "Power Count" + "\n        " + GameControl.control.powerUpCount);

		GUI.Label(new Rect((Screen.width) - 85, 50, 150, 60), "Focus Modes" + "\n        " + GameControl.control.focusModeCount);

		GUI.Label(new Rect(10, 10, 150, 30), "Score: " + (int)(playerScore * 100));

		GUI.Label(new Rect(10, Screen.height - 20, 100, 30), "Player: " + GameControl.control.playerName);

		if(script_Restarter.gameOver)
		{
			if(waitingForScoreToPost)
			{
				GUI.Label(new Rect((Screen.width / 2) - 50, 275, 100, 30), "Posting Score...");

				if(script_SubmitHighScore.DonePosting())
				{
					Time.timeScale = 1;
					Application.LoadLevel("HighScores");
				}
			}
			else
			{
				if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 55, 100, 50), "Submit Score")) 
				{
					script_SubmitHighScore.SubmitScore(GameControl.control.name, (int)(playerScore * 100));
					waitingForScoreToPost = true;
				}
				
				if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2), 100, 50), "Play Again")) 
				{
					Time.timeScale = 1;
					Application.LoadLevel(Application.loadedLevelName);
				}
				
				if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) + 55, 100, 50), "Exit")) 
				{
					Time.timeScale = 1;
					Application.LoadLevel("main");
				}
			}
		}
	}
}
