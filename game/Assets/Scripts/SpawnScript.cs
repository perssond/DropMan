using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour 
{
	public GameObject[] ground;
	public GameObject[] groundNoCollider;

	protected int lastSpawn = 1;

	public bool useBoxColliders = true;

	public void Awake()
	{
	}

	public void Update()
	{
	}

	// Update is called once per frame
	public void Spawn() 
	{
		int nextSpawn = Random.Range(0, ground.GetLength(0));

		while(nextSpawn == lastSpawn)
		{
			nextSpawn = Random.Range(0, ground.GetLength(0));
		}

		lastSpawn = nextSpawn;

		if(useBoxColliders)
		{
			Instantiate(ground[nextSpawn], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
		}
		else
		{
			Instantiate(groundNoCollider[nextSpawn], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
		}
	}

	public void SetUsingColliders(bool use)
	{
		useBoxColliders = use;
	}
}
