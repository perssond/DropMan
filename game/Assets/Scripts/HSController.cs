using UnityEngine;
using System.Collections;

public class HSController : MonoBehaviour
{
	public Transform trans_GameIntroMusic;
	
	protected StayAwakeSound script_GameIntroMusic;

	private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
	private string addScoreURL = "http://dropman.atwebpages.com/addscore.php?"; //be sure to add a ? to your url
	private string highscoreURL = "http://dropman.atwebpages.com/display.php";

	public GUIText textObject;

	MD5Testing MD5Test = new MD5Testing();

	void Start()
	{
		GameControl.control.Load();

		textObject = this.gameObject.GetComponent<GUIText>();

		trans_GameIntroMusic = Instantiate(trans_GameIntroMusic) as Transform;
		script_GameIntroMusic = trans_GameIntroMusic.gameObject.GetComponent<StayAwakeSound>();

		script_GameIntroMusic.SetSoundTime(GameControl.control.gameIntroMusicMS);
		script_GameIntroMusic.Play(true);

		StartCoroutine(GetScores());
	}

	void OnGUI()
	{
//		if(GUI.Button(new Rect(380,275,140,80), "Save")) 
//		{
//			StartCoroutine(PostScores("", 5));
//		}
//
//		if(GUI.Button(new Rect(380,375,140,80), "Load")) 
//		{
//			StartCoroutine(GetScores());
//		}

		if(GUI.Button(new Rect(10,30,80,30), "Back")) 
		{
			GameControl.control.gameIntroMusicMS = script_GameIntroMusic.GetSoundTime();
			GameControl.control.Save();
			Application.LoadLevel("main");
		}

		GUI.Label(new Rect(10, Screen.height - 20, 100, 30), "Player: " + GameControl.control.playerName);
	}
	
	// remember to use StartCoroutine when calling this function!
	IEnumerator PostScores(string name, int score)
	{
		//This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.
		string hash = MD5Test.Md5Sum(name + score + secretKey);
		
		string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
		
		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done
		
		if (hs_post.error != null)
		{
			print("There was an error posting the high score: " + hs_post.error);
		}
	}
	
	// Get the scores from the MySQL DB to display in a GUIText.
	// remember to use StartCoroutine when calling this function!
	IEnumerator GetScores()
	{
		textObject.text = "Loading Scores";

		WWW hs_get = new WWW(highscoreURL);
		yield return hs_get;

		if (hs_get.error != null)
		{
			print("There was an error getting the high score: " + hs_get.error);
		}
		else
		{
			textObject.text = hs_get.text; // this is a GUIText that will display the scores in game.
		}
	}
}

public class MD5Testing
{
	public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}
}

