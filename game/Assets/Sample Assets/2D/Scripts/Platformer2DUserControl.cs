using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour 
{
	public Transform trans_Fart1;
	public Transform trans_Fart2;

	private PlatformerCharacter2D character;
	protected StayAwakeSound script_Fart1;
	protected StayAwakeSound script_Fart2;    

	private bool tootScootActive;

	private float tootScootStart;
	private float tootScootCoolDownTimer;
	private float tootScootCoolDown = 0.25f;
	private float tootScootDuration = 0.035f;

	protected bool playedFart1 = false;

	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();

		trans_Fart1 = Instantiate(trans_Fart1) as Transform;
		script_Fart1 = trans_Fart1.gameObject.GetComponent<StayAwakeSound>();	
		
		trans_Fart2 = Instantiate(trans_Fart2) as Transform;
		script_Fart2 = trans_Fart2.gameObject.GetComponent<StayAwakeSound>();	
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.

		// NO JUMPING IN DROP MAN ONLY DROPPING
		//if (CrossPlatformInput.GetButtonDown("Jump"))
          //jump = true;

		if (CrossPlatformInput.GetButtonDown("Jump") && !tootScootActive && (Time.time > tootScootCoolDownTimer))
		{
			tootScootStart =  Time.time;
			tootScootActive = true;	

			if(!playedFart1)
			{
				script_Fart1.Play(false);
				playedFart1 = true;
			}
			else
			{
				script_Fart2.Play(false);
				playedFart1 = false;
			}
		}
	}

	void FixedUpdate()
	{
		// Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);

		float h = CrossPlatformInput.GetAxis("Horizontal");

		// Pass all parameters to the character control script.
		character.Move(h, false , false, tootScootActive);

		if(tootScootActive && Time.time > (tootScootStart + tootScootDuration))
		{
			tootScootActive = false;
			tootScootCoolDownTimer = Time.time + tootScootCoolDown;
		}
        // Reset the jump input once it has been used.
	    //jump = false;
	}
}
