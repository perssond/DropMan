using UnityEngine;
using System.Collections;

public class StayAwakeSound : MonoBehaviour 
{	
	protected int SoundLoopCounter;
	protected float fadeSpeed = 0.0f;
	protected float VolFadeValue = 0.0f;
	
	public enum SoundStates
	{
		None,
		FadingIn,
		FadingOut,
	}	
	
	protected SoundStates SoundState;
	
	public void Awake()
	{
		SoundLoopCounter = 0;
		SoundState = SoundStates.None;
	}
	
	public void Update() 
	{
		if(SoundLoopCounter > 0 && !transform.GetComponent<AudioSource>().isPlaying)
		{
 			transform.GetComponent<AudioSource>().Play();
			SoundLoopCounter--;
		}
		
		if(SoundState == SoundStates.FadingIn)
		{
			VolFadeValue += Mathf.Clamp01(Time.deltaTime / fadeSpeed);
			
			transform.GetComponent<AudioSource>().volume = VolFadeValue;
						
			if(VolFadeValue >= 1.0f)
				SoundState = SoundStates.None;
		}
		else if(SoundState == SoundStates.FadingOut)
		{
			VolFadeValue -= Mathf.Clamp01(Time.deltaTime / fadeSpeed);
			
			transform.GetComponent<AudioSource>().volume = VolFadeValue;
			
			if(VolFadeValue <= 0.0f)
				SoundState = SoundStates.None;			
		}		
	}
	
	public void Play(bool loop) 
	{
		if(loop)
			transform.GetComponent<AudioSource>().loop = true;
		else
			transform.GetComponent<AudioSource>().loop = false;
		
		transform.GetComponent<AudioSource>().Play();
	}
	
	public void Stop() 
	{
		transform.GetComponent<AudioSource>().Stop();
	}
	
	public void Loop(int numTimes)
	{
		SoundLoopCounter = numTimes;
	}
	
	public bool IsPlaying()
	{
		return transform.GetComponent<AudioSource>().isPlaying;
	}
	
	public void SetSoundTime(float inTime)
	{
		transform.GetComponent<AudioSource>().time = inTime;
	}	
	
	public float GetSoundTime()
	{
		return transform.GetComponent<AudioSource>().time;
	}
	
	public void FadeIn (float speed, bool loop)
	{			
		transform.GetComponent<AudioSource>().loop = loop;

		fadeSpeed = speed;
		VolFadeValue = 0.0f;
		transform.GetComponent<AudioSource>().Play();
		
		SoundState = SoundStates.FadingIn;
	}	
	
	public void FadeOut (float speed)
	{
		fadeSpeed = speed;
		VolFadeValue = 1.0f;
		
		SoundState = SoundStates.FadingOut;
	}
	
	public void SetVolume (float vol)
	{
		transform.GetComponent<AudioSource>().volume = vol;
	}	
	
	public bool WorkComplete()
	{
		return SoundState == SoundStates.None;	
	}	
}
