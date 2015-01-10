using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		private readonly object lockerObject = new object();
		public BananaSpawner BananaSpawner;
		[HideInInspector] public GameState CurrentGameState;
		private int currentRoundIndex;
		public List<GameObject> Enemies;
		public GameObject EnemyEasyPrefab;
		public GameObject EnemyHardPrefab;
		public GameObject EnemyNormalPrefab;
		[HideInInspector] public bool FinalRoundFinished;
		public GUIText InfoText;
		private LevelStuffFromXML levelStuffFromXML;
		public int Lives = 10;
		[HideInInspector] public float MaxBananaSpawnTime;
		[HideInInspector] public float MinBananaSpawnTime;
		public SpriteRenderer MonkeyFastGeneratorSprite;
		public SpriteRenderer MonkeyMediumGeneratorSprite;
		public List<GameObject> Monkeys;
		public SpriteRenderer MonkeySlowGeneratorSprite;
		public GameObject MonkeyUpgraderDamage;
		public GameObject MonkeyUpgraderRange;
		public GameObject MonkeyUpgraderSpeed;
		private GameObject pathPiecesParent;
		public GameObject PathPrefab;
		public GameObject TowerPrefab;
		public Transform[] Waypoints;
		private GameObject waypointsParent;
		public int gameLevel;

		[HideInInspector]
		public static GameManager Instance { get; private set; }

		[HideInInspector]
		public int MoneyAvailable { get; private set; }

		public void AlterMoneyAvailable(int money)
		{
			MoneyAvailable += money;

			var medium = MonkeyMediumGeneratorSprite.color;
			var fast = MonkeyFastGeneratorSprite.color;
			var slow = MonkeySlowGeneratorSprite.color;

			medium.a = MoneyAvailable < Constants.MonkeyMediumCost ? 0.3f : 1.0f;
			fast.a = MoneyAvailable < Constants.MonkeyFastCost ? 0.3f : 1.0f;
			slow.a = MoneyAvailable < Constants.MonkeySlowCost ? 0.3f : 1.0f;

			MonkeyMediumGeneratorSprite.color = medium;
			MonkeyFastGeneratorSprite.color = fast;
			MonkeySlowGeneratorSprite.color = slow;
		}

		public void Awake()
		{
			Instance = this;
		}

		public void Start()
		{
			//const int gameLevel = 5;
			IgnoreLayerCollisions();

			Enemies = new List<GameObject>();
			pathPiecesParent = GameObject.Find("PathPieces");
			waypointsParent = GameObject.Find("Waypoints");
			levelStuffFromXML = Utilities.ReadXMLFile(gameLevel);

			CreateLevelFromXML();

			CurrentGameState = GameState.Start;

			FinalRoundFinished = false;
		}

		private void CreateLevelFromXML()
		{
			foreach (var go in levelStuffFromXML.Paths.Select(position => Instantiate(PathPrefab, position,
				Quaternion.identity) as GameObject))
			{
				go.GetComponent<SpriteRenderer>().sortingLayerName = "Path";
				go.transform.parent = pathPiecesParent.transform;
			}

			for (var i = 0; i < levelStuffFromXML.Waypoints.Count; i++)
			{
				var go = new GameObject();
				go.transform.position = levelStuffFromXML.Waypoints[i];
				go.transform.parent = waypointsParent.transform;
				go.tag = "Waypoint";
				go.name = "Waypoints" + i;
			}

			var tower = Instantiate(TowerPrefab, levelStuffFromXML.Tower,
				Quaternion.identity) as GameObject;
			if (tower != null)
				tower.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

			Waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
				.OrderBy(x => x.name).Select(x => x.transform).ToArray();

			MoneyAvailable = levelStuffFromXML.InitialMoney;
			MinBananaSpawnTime = levelStuffFromXML.MinBananaSpawnTime;
			MaxBananaSpawnTime = levelStuffFromXML.MaxBananaSpawnTime;
		}

		private static void IgnoreLayerCollisions()
		{
			var monkeyLayerId = LayerMask.NameToLayer("Monkey");
			var enemyLayerId = LayerMask.NameToLayer("Enemy");
			var arrowLayerId = LayerMask.NameToLayer("Arrow");
			var monkeyGeneratorLayerId = LayerMask.NameToLayer("MonkeyGenerator");
			var backgroundLayerId = LayerMask.NameToLayer("Background");
			var pathLayerId = LayerMask.NameToLayer("Path");
			var towerLayerId = LayerMask.NameToLayer("Tower");
			var bananaLayerId = LayerMask.NameToLayer("Banana");
			var monkeyUpgraderLayerId = LayerMask.NameToLayer("MonkeyUpgrader");

			Physics2D.IgnoreLayerCollision(monkeyLayerId, enemyLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, monkeyGeneratorLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, backgroundLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, pathLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, monkeyLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, towerLayerId);
			Physics2D.IgnoreLayerCollision(arrowLayerId, bananaLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, monkeyLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, enemyLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, arrowLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, pathLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, bananaLayerId);
			Physics2D.IgnoreLayerCollision(monkeyUpgraderLayerId, backgroundLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, monkeyLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, enemyLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, monkeyGeneratorLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, backgroundLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, pathLayerId);
			Physics2D.IgnoreLayerCollision(bananaLayerId, towerLayerId);
		}

		private IEnumerator NextRound()
		{
			//give the player 2 secs to do stuff
			yield return new WaitForSeconds(2f);

			//get a reference to the next round details
			var currentRound = levelStuffFromXML.Rounds[currentRoundIndex];

			for (var i = 0; i < currentRound.NoOfEnemiesEasy; i++)
			{
				//spawn a new enemy
				var enemy = Instantiate(EnemyEasyPrefab, Waypoints[0].position, Quaternion.identity) as GameObject;
				if (enemy != null)
				{
					var enemyComponent = enemy.GetComponent<EnemyEasy>();
					//set speed and enemyKilled handler
					enemyComponent.Speed += Mathf.Clamp(currentRoundIndex, 1f, 5f);
					enemyComponent.EnemyKilled += OnEnemyKilled;
				}
				//add it to the list and wait till you spawn the next one
				Enemies.Add(enemy);
				yield return new WaitForSeconds(1f/(currentRoundIndex == 0 ? 1 : currentRoundIndex));
			}
			for (var i = 0; i < currentRound.NoOfEnemiesNormal; i++)
			{
				//spawn a new enemy
				var enemy = Instantiate(EnemyNormalPrefab, Waypoints[0].position, Quaternion.identity) as GameObject;
				if (enemy != null)
				{
					var enemyComponent = enemy.GetComponent<EnemyNormal>();
					//set speed and enemyKilled handler
					enemyComponent.Speed += Mathf.Clamp(currentRoundIndex, 1f, 5f);
					enemyComponent.EnemyKilled += OnEnemyKilled;
				}
				//add it to the list and wait till you spawn the next one
				Enemies.Add(enemy);
				yield return new WaitForSeconds(1f/(currentRoundIndex == 0 ? 1 : currentRoundIndex));
			}
			for (var i = 0; i < currentRound.NoOfEnemiesHard; i++)
			{
				//spawn a new enemy
				var enemy = Instantiate(EnemyHardPrefab, Waypoints[0].position, Quaternion.identity) as GameObject;
				if (enemy != null)
				{
					var enemyComponent = enemy.GetComponent<EnemyHard>();
					//set speed and enemyKilled handler
					enemyComponent.Speed += Mathf.Clamp(currentRoundIndex, 1f, 5f);
					enemyComponent.EnemyKilled += OnEnemyKilled;
				}
				//add it to the list and wait till you spawn the next one
				Enemies.Add(enemy);
				yield return new WaitForSeconds(1f/(currentRoundIndex == 0 ? 1 : currentRoundIndex));
			}
		}

		private void OnEnemyKilled(object sender, EventArgs e)
		{
			var startNewRound = false;
			//explicit lock, since this may occur any time by any enemy
			//not 100% that this is needed, but better safe than sorry!
			lock (lockerObject)
			{
				if (Enemies.Count(x => x != null) == 0 && CurrentGameState == GameState.Playing)
					startNewRound = true;
			}
			if (startNewRound)
				CheckAndStartNewRound();
		}

		private void CheckAndStartNewRound()
		{
			if (currentRoundIndex < levelStuffFromXML.Rounds.Count - 1)
			{
				currentRoundIndex++;
				StartCoroutine(NextRound());
			}
			else
				FinalRoundFinished = true;
		}

		public void Update()
		{
			switch (CurrentGameState)
			{
				//start state, on tap, start the game and spawn carrots!
				case GameState.Start:
					if (Input.GetMouseButtonUp(0))
					{
						CurrentGameState = GameState.Playing;
						StartCoroutine(NextRound());
						BananaSpawner.StartBananaSpawning();
					}
					break;

				case GameState.Playing:
					if (Lives <= 0) //we lost
					{
						//no more rounds
						StopCoroutine(NextRound());
						DestroyExistingEnemiesAndBananas();
						BananaSpawner.StopBananaSpawning();
						CurrentGameState = GameState.Lost;
					}
					else if (FinalRoundFinished && Enemies.Count(x => x != null) == 0)
					{
						DestroyExistingEnemiesAndBananas();
						BananaSpawner.StopBananaSpawning();
						CurrentGameState = GameState.Won;
					}
					break;

				case GameState.Won:
					if (Input.GetMouseButtonUp(0))
						//restart
						Application.LoadLevel(Application.loadedLevel);
					break;

				case GameState.Lost:
					if (Input.GetMouseButtonUp(0))
						//restart
						Application.LoadLevel(Application.loadedLevel);
					break;
			}
		}

		public void ShowMonkeyUpgrader(MonkeyBase monkey)
		{
			MonkeyUpgraderDamage.GetComponent<MonkeyUpgraderDamage>().MonkeyBase = monkey;
			MonkeyUpgraderRange.GetComponent<MonkeyUpgraderRange>().MonkeyBase = monkey;
			MonkeyUpgraderSpeed.GetComponent<MonkeyUpgraderSpeed>().MonkeyBase = monkey;
			MonkeyUpgraderDamage.GetComponent<MonkeyUpgraderDamage>().Show();
			MonkeyUpgraderRange.GetComponent<MonkeyUpgraderRange>().Show();
			MonkeyUpgraderSpeed.GetComponent<MonkeyUpgraderSpeed>().Show();
		}

		public void HideMonkeyUpgrade()
		{
			MonkeyUpgraderDamage.GetComponent<MonkeyUpgraderDamage>().Hide();
			MonkeyUpgraderRange.GetComponent<MonkeyUpgraderRange>().Hide();
			MonkeyUpgraderSpeed.GetComponent<MonkeyUpgraderSpeed>().Hide();
		}

		private void DestroyExistingEnemiesAndBananas()
		{
			//get all the enemies
			foreach (var item in Enemies.Where(item => item != null))
				Destroy(item.gameObject);
			//get all the carrots
			var bananas = GameObject.FindGameObjectsWithTag("Banana");
			foreach (var item in bananas)
				Destroy(item);
		}

		public void OnGUI()
		{
			Utilities.AutoResize(800, 480);
			switch (CurrentGameState)
			{
				case GameState.Start:
					InfoText.text = "Tap to start!";
					break;

				case GameState.Playing:
					InfoText.text = "Money: " + MoneyAvailable + "\n"
					                + "Life: " + Lives + "\n" +
					                string.Format("round {0} of {1}", currentRoundIndex + 1, levelStuffFromXML.Rounds.Count);
					break;

				case GameState.Won:
					InfoText.text = "Won! Tap to restart!";
					break;

				case GameState.Lost:
					InfoText.text = "Lost :( Tap to restart!";
					break;
			}
		}
	}
}