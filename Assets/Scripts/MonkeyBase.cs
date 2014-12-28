using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyBase : MonoBehaviour
	{
		public GameObject ArrowPrefab;
		public Transform ArrowSpawnPosition;
		public int Damage;
		private bool isMouseDown;
		private float lastShootTime;
		public int LevelDamage;
		public int LevelRange;
		public int LevelSpeed;
		public float Range;
		public float Speed;
		private MonkeyState state;
		private GameObject targetedEnemy;

		public int UpgradeCostSpeed
		{
			get { return 0; }
		}

		public int UpgradeCostRange
		{
			get { return 0; }
		}

		public int UpgradeCostDamage
		{
			get { return 0; }
		}

		public void Activate()
		{
			state = MonkeyState.Searching;
		}

		public void Start()
		{
			state = MonkeyState.Inactive;
			LevelSpeed = 0;
			LevelRange = 0;
			LevelDamage = 0;
			ArrowSpawnPosition = transform.FindChild("ArrowSpawnPosition");
		}

		public void Update()
		{
			//if we're in the last round and we've killed all enemies, do nothing
			if (GameManager.Instance.FinalRoundFinished &&
			    GameManager.Instance.Enemies.Count(x => x != null) == 0)
			{
				state = MonkeyState.Inactive;
				return;
			}

			var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Input.GetMouseButtonDown(0) &&
			    GetComponent<CircleCollider2D>() ==
			    Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Monkey")))
				isMouseDown = true;
			else if (Input.GetMouseButtonUp(0) && isMouseDown && GetComponent<CircleCollider2D>() ==
			         Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Monkey")))
			{
				GameManager.Instance.ShowMonkeyUpgrader(this);
				isMouseDown = false;
			}
			else
				isMouseDown = false;

			//searching for an enemy
			switch (state)
			{
				case MonkeyState.Searching:
					if (GameManager.Instance.Enemies.Count(x => x != null) == 0)
						return;

					targetedEnemy = GameManager.Instance.Enemies.Where(x => x != null)
						.Aggregate((current, next) => Vector2.Distance(current.transform.position, transform.position)
						                              < Vector2.Distance(next.transform.position, transform.position)
							? current
							: next);

					if (targetedEnemy != null && targetedEnemy.activeSelf
					    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
					    < Range)
						state = MonkeyState.Targeting;
					break;

				case MonkeyState.Targeting:
					if (targetedEnemy != null
					    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
					    < Range)
						LookAndShoot();
					else //enemy has left our shooting range, so look for another one
						state = MonkeyState.Searching;
					break;
			}
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

			if (!(Time.time - lastShootTime > Speed))
				return;

			Shoot(direction);
			lastShootTime = Time.time;
		}

		private void Shoot(Vector2 dir)
		{
			//if the enemy is still close to us
			if (targetedEnemy != null && targetedEnemy.activeSelf
			    && Vector3.Distance(transform.position, targetedEnemy.transform.position)
			    < Range)
			{
				//create a new arrow
				var go = GetPooledArrow();
				go.GetComponent<ArrowBase>().ArrowDamage = Damage;
				go.transform.position = ArrowSpawnPosition.position;
				go.transform.rotation = transform.rotation;
				go.SetActive(true);

				//SHOOT IT!
				go.GetComponent<Rigidbody2D>().AddForce(dir*Constants.ArrowSpeed);
				AudioManager.Instance.PlayArrowSound();
			}
			else //find another enemy
				state = MonkeyState.Searching;
		}

		protected virtual GameObject GetPooledArrow()
		{
			throw new NotImplementedException();
		}
	}
}