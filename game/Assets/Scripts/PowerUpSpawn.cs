using UnityEngine;
using System.Collections;

public class PowerUpSpawn : MonoBehaviour 
{
	public GameObject powerUp;

	// Update is called once per frame
	public void Spawn() 
	{
		Instantiate(powerUp, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
	}
}