using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour 
{
	public static GameControl control;

	public int score;
	public int powerUpCount = 0;
	public int focusModeCount = 0;
	public float gamePlayMusicMS = 0.0f;
	public float gameIntroMusicMS = 0.0f;
	public bool fullScreenMode;

	public string playerName;

	// Use this for initialization. Awake happens before start happens.
	void Awake() 
	{
		if(control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy(gameObject);
		}

		powerUpCount = 0;
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData();
		data.score = score;
		data.gamePlayMusicMS = gamePlayMusicMS;
		data.fullScreenMode = fullScreenMode;
		data.gameIntroMusicMS = gameIntroMusicMS;
		data.playerName = playerName;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			score = data.score;
			gamePlayMusicMS = data.gamePlayMusicMS;
			gameIntroMusicMS = data.gameIntroMusicMS;
			fullScreenMode = data.fullScreenMode;

			playerName = data.playerName;
		}
	}

	void OnGUI()
	{
		if(Application.loadedLevelName != "DropMan")
		{
			fullScreenMode = GUI.Toggle(new Rect (0,0,150,20), fullScreenMode, "Full Screen");

			if(Screen.fullScreen && !fullScreenMode)
			{
				Screen.fullScreen = false;
				Save();
			}
			else if(!Screen.fullScreen && fullScreenMode)
			{
				Screen.fullScreen = true;
				Save();
			}
		}
	}

	void OnApplicationQuit()
	{
		gamePlayMusicMS = 0.0f;
		gameIntroMusicMS = 0.0f;

		fullScreenMode = Screen.fullScreen;

		score = 0;

		Save();
	}
}

[Serializable]
class PlayerData
{
	public int score;
	public String playerName;
	public float gamePlayMusicMS;
	public float gameIntroMusicMS;
	public bool fullScreenMode;
}
