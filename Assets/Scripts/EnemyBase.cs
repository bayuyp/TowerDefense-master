using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyBase : MonoBehaviour
	{
		public int Bounty;
		public int Health;
		private int nextWaypointIndex;
		public float Speed;
		public event EventHandler EnemyKilled;
		public void Start() {}

		public void Update()
		{
			if (Vector2.Distance(transform.position,
				GameManager.Instance.Waypoints[nextWaypointIndex].position) < 0.01f)

				if (nextWaypointIndex == GameManager.Instance.Waypoints.Length - 1)
				{
					RemoveAndDestroy();
					GameManager.Instance.Lives--;
				}
				else
				{
					nextWaypointIndex++;
					transform.LookAt(GameManager.Instance.Waypoints[nextWaypointIndex].position,
						-Vector3.forward);
					transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
				}

			transform.position = Vector2.MoveTowards(transform.position,
				GameManager.Instance.Waypoints[nextWaypointIndex].position,
				Time.deltaTime*Speed);
		}

		public void OnCollisionEnter2D(Collision2D col)
		{
			if (!col.gameObject.tag.Equals("Arrow"))
				return;

			if (Health > 0)
			{
				Health -= col.gameObject.GetComponent<ArrowBase>().ArrowDamage;
				if (Health <= 0)
					RemoveAndDestroy();
			}
			col.gameObject.GetComponent<ArrowBase>().Disable();
		}

		private void RemoveAndDestroy()
		{
			AudioManager.Instance.PlayDeathSound();
			GameManager.Instance.Enemies.Remove(gameObject);
			Destroy(gameObject);
			GameManager.Instance.AlterMoneyAvailable(Bounty);

			if (EnemyKilled != null)
				EnemyKilled(this, EventArgs.Empty);
		}
	}
}