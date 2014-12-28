using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class LevelStuffFromXML
	{
		public int InitialMoney;
		public float MaxBananaSpawnTime;
		public float MinBananaSpawnTime;
		public List<Vector2> Paths;
		public List<Round> Rounds;
		public Vector2 Tower;
		public List<Vector2> Waypoints;

		public LevelStuffFromXML()
		{
			Paths = new List<Vector2>();
			Waypoints = new List<Vector2>();
			Rounds = new List<Round>();
		}
	}

	public class Round
	{
		public int NoOfEnemies { get; set; }
		public int NoOfEnemiesEasy { get; set; }
		public int NoOfEnemiesNormal { get; set; }
		public int NoOfEnemiesHard { get; set; }
	}
}