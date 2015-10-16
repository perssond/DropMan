using UnityEngine;
using System.Collections;

public class ScrollingObject : MonoBehaviour 
{
	protected float moveSpeed = 4.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.gameObject.transform.position.y > 6.0)
		{
			Destroy(this.gameObject);
		}

		transform.Translate(new Vector3(0, moveSpeed, 0) * Time.deltaTime);
	}
}
