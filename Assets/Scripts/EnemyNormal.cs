using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts;
using System;

public class EnemyNormal : MonoBehaviour
{
    //death sound found here
    //https://www.freesound.org/people/psychentist/sounds/168567/

    public int Health;
	public int Bounty;
	public float Speed;
	int nextWaypointIndex = 0;
	// Use this for initialization

	//public Transform ArrowSpawnPosition;
	//public GameObject ArrowPrefab;

	/*public float ShootWaitTime = 2f;
	private float LastShootTime = 0f;
	GameObject targetedEnemy;
	private float InitialArrowForce = 500f;*/

    void Start()
    {
        Health = Constants.InitialEnemyNormalHealth;
		Bounty = Constants.NormalEnemyBounty;
		Speed = Constants.SpeedyEnemyNormal;
    }

    // Update is called once per frame
    void Update()
    {

				//calculate the distance between current position
				//and the target waypoint
				if (Vector2.Distance (transform.position,
            GameManager.Instance.Waypoints [nextWaypointIndex].position) < 0.01f) {
						//is this waypoint the last one?
						if (nextWaypointIndex == GameManager.Instance.Waypoints.Length - 1) {
								RemoveAndDestroy ();
								GameManager.Instance.Lives--;
						} else {
								//our enemy will go to the next waypoint
								nextWaypointIndex++;
								//our simple AI, enemy is looking at the next waypoint
								transform.LookAt (GameManager.Instance.Waypoints [nextWaypointIndex].position,
                    -Vector3.forward);
								//only in the z axis
								transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
						}
				}
        
				//enemy is moved towards the next waypoint
				transform.position = Vector2.MoveTowards (transform.position,
            GameManager.Instance.Waypoints [nextWaypointIndex].position,
            Time.deltaTime * Speed);
		}

    void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.tag == "Arrow")
		{//if we're hit by an arrow
			if (Health > 0)
			{
				//decrease enemy health
				Health -= Constants.ArrowDamage;
				if (Health <= 0)
				{
					RemoveAndDestroy();
				}
			}
			col.gameObject.GetComponent<Arrow>().Disable(); //disable the arrow
		}
		
		if (col.gameObject.tag == "ArrowFast")
		{//if we're hit by an arrow
			if (Health > 0)
			{
				//decrease enemy health
				Health -= Constants.ArrowFastDamage;
				if (Health <= 0)
				{
					RemoveAndDestroy();
				}
			}
			col.gameObject.GetComponent<Arrow>().Disable(); //disable the arrow
		}
		
		if (col.gameObject.tag == "ArrowSlow")
		{//if we're hit by an arrow
			if (Health > 0)
			{
				//decrease enemy health
				Health -= Constants.ArrowSlowDamage;
				if (Health <= 0)
				{
					RemoveAndDestroy();
				}
			}
			col.gameObject.GetComponent<Arrow>().Disable(); //disable the arrow
		}

    }

    public event EventHandler EnemyKilled;

    void RemoveAndDestroy()
    {
        AudioManager.Instance.PlayDeathSound();
        //remove it from the enemy list
        GameManager.Instance.Enemies.Remove(this.gameObject);
        Destroy(this.gameObject);
		GameManager.Instance.AlterMoneyAvailable (Bounty);
        //notify interested parties that we died
        if (EnemyKilled != null)
            EnemyKilled(this, EventArgs.Empty);
    }
}
