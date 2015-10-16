using UnityEngine;
using System.Collections;

public class SubmitHighScore : MonoBehaviour 
{
	private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
	private string addScoreURL = "http://dropman.atwebpages.com/addscore.php?"; //be sure to add a ? to your url

	MD5Testing MD5Test = new MD5Testing();

	private bool donePosting = false;

	public void SubmitScore(string name, int score)
	{
		Debug.Log("Submitting: " + score);
		donePosting = false;
		StartCoroutine(PostScores(GameControl.control.playerName, score));
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

		donePosting = true;

		if (hs_post.error != null)
		{
			print("There was an error posting the high score: " + hs_post.error);
		}
	}

	public bool DonePosting()
	{
		return donePosting;
	}
}