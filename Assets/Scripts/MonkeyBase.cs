using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyBase : MonoBehaviour
	{
		private const float InitialArrowForce = 500f;
		public GameObject ArrowPrefab;
		public Transform ArrowSpawnPosition;
		private float lastShootTime;
		public float ShootWaitTime;
		private MonkeyState state;
		private GameObject targetedEnemy;

		public void Activate()
		{
			state = MonkeyState.Searching;
		}

		public void Start()
		{
			state = MonkeyState.Inactive;
			//find where we're shooting from
			ArrowSpawnPosition = transform.FindChild("ArrowSpawnPosition");
		}

		public void Update()
		{
			//if we're in the last round and we've killed all enemies, do nothing
			if (GameManager.Instance.FinalRoundFinished &&
			    GameManager.Instance.Enemies.Count(x => x != null) == 0)
				state = MonkeyState.Inactive;

			//searching for an enemy
			if (state == MonkeyState.Searching)
			{
				if (GameManager.Instance.Enemies.Count(x => x != null) == 0)
					return;

				//find the closest enemy
				targetedEnemy = GameManager.Instance.Enemies.Where(x => x != null)
					.Aggregate((current, next) => Vector2.Distance(current.transform.position, transform.position)
					                              < Vector2.Distance(next.transform.position, transform.position)
						? current
						: next);

				//if there is an enemy and is close to us, target it
				if (targetedEnemy != null && targetedEnemy.activeSelf
				    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
				    < Constants.MinDistanceForMonkeyToShoot)
					state = MonkeyState.Targeting;
			}
			else if (state == MonkeyState.Targeting)
				//if the targeted enemy is still close to us, look at it and shoot!
				if (targetedEnemy != null
				    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
				    < Constants.MinDistanceForMonkeyToShoot)
					LookAndShoot();
				else //enemy has left our shooting range, so look for another one
					state = MonkeyState.Searching;
		}

		private void LookAndShoot()
		{
			//look at the enemy
			var diffRotation = Quaternion.LookRotation
				(transform.position - targetedEnemy.transform.position, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards
				(transform.rotation, diffRotation, Time.deltaTime*2000);
			transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

			//make sure we're almost looking at the enemy before start shooting
			Vector2 direction = targetedEnemy.transform.position - transform.position;
			var axisDif = Vector2.Angle(transform.up, direction);
			//shoot only if we have 20 degrees rotation difference to the enemy
			if (!(axisDif <= 20f))
				return;

			if (!(Time.time - lastShootTime > ShootWaitTime))
				return;

			Shoot(direction);
			lastShootTime = Time.time;
		}

		private void Shoot(Vector2 dir)
		{
			//if the enemy is still close to us
			if (targetedEnemy != null && targetedEnemy.activeSelf
			    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
			    < Constants.MinDistanceForMonkeyToShoot)
			{
				//create a new arrow
				var go = ObjectPoolerManager.Instance.ArrowFastPooler.GetPooledObject();
				go.transform.position = ArrowSpawnPosition.position;
				go.transform.rotation = transform.rotation;
				go.SetActive(true);
				//SHOOT IT!
				go.GetComponent<Rigidbody2D>().AddForce(dir*InitialArrowForce);
				AudioManager.Instance.PlayArrowSound();
			}
			else //find another enemy
				state = MonkeyState.Searching;
		}
	}
}