using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public static class Utilities
	{
		public static void AutoResize(int screenWidth, int screenHeight)
		{
			var resizeRatio = new Vector2((float) Screen.width/screenWidth, (float) Screen.height/screenHeight);
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
		}

		public static LevelStuffFromXML ReadXMLFile(int lvl)
		{
			var stringLevel = "";
			switch (lvl)
			{
				case 1:
					stringLevel = "Level1";
					break;
				case 2:
					stringLevel = "Level2";
					break;
				case 3:
					stringLevel = "Level3";
					break;
				case 4:
					stringLevel = "Level4";
					break;
				case 5:
					stringLevel = "Level5";
					break;
				case 6:
					stringLevel = "Level6";
					break;
				case 7:
					stringLevel = "Level7";
					break;
				case 8:
					stringLevel = "Level8";
					break;
				case 9:
					stringLevel = "Level9";
					break;
				case 10:
					stringLevel = "Level10";
					break;
				case 11:
				stringLevel = "Level11";
					break;
				case 12:
				stringLevel = "Level12";
					break;
				case 13:
				stringLevel = "Level13";
					break;
				case 14:
				stringLevel = "Level14";
					break;
				case 15:
				stringLevel = "Level15";
					break;
			}

			var ls = new LevelStuffFromXML();
			var ta = Resources.Load(stringLevel) as TextAsset;
			if (ta != null)
			{
				var xdoc = XDocument.Parse(ta.text);
				var el = xdoc.Element("Elements");
				if (el != null)
				{
					var xElement = el.Element("PathPieces");
					if (xElement != null)
					{
						var paths = xElement.Elements("Path");

						foreach (var item in paths)
						{
							var xAttribute = item.Attribute("X");
							if (xAttribute != null)
							{
								var attribute = item.Attribute("Y");
								if (attribute != null)
									ls.Paths.Add(new Vector2(float.Parse(xAttribute.Value), float.Parse(attribute.Value)));
							}
						}
					}
				}

				if (el != null)
				{
					var xElement = el.Element("Waypoints");
					if (xElement != null)
					{
						var waypoints = xElement.Elements("Waypoint");
						foreach (var item in waypoints)
						{
							var xAttribute = item.Attribute("X");
							if (xAttribute != null)
							{
								var attribute = item.Attribute("Y");
								if (attribute != null)
									ls.Waypoints.Add(new Vector2(float.Parse(xAttribute.Value), float.Parse(attribute.Value)));
							}
						}
					}
				}

				if (el != null)
				{
					var xElement = el.Element("Rounds");
					if (xElement != null)
					{
						var rounds = xElement.Elements("Round");

						foreach (var item in rounds)
							//int num = int.Parse(item.Attribute("NoOfEnemies").Value);
							//ls.Rounds.Add(new Round(num));

						{
							var xAttribute = item.Attribute("NoOfEnemiesEasy");
							if (xAttribute != null)
							{
								var attribute = item.Attribute("NoOfEnemiesNormal");
								if (attribute != null)
								{
									var xAttribute1 = item.Attribute("NoOfEnemiesHard");
									if (xAttribute1 != null)
										ls.Rounds.Add(new Round
										{
											//NoOfEnemies = int.Parse(item.Attribute("NoOfEnemies").Value)
											NoOfEnemiesEasy = int.Parse(xAttribute.Value),
											NoOfEnemiesNormal = int.Parse(attribute.Value),
											NoOfEnemiesHard = int.Parse(xAttribute1.Value)
										});
								}
							}
						}
					}
				}

				if (el != null)
				{
					var tower = el.Element("Tower");
					if (tower != null)
					{
						var xAttribute = tower.Attribute("Y");
						if (xAttribute != null)
						{
							var attribute = tower.Attribute("X");
							if (attribute != null)
								ls.Tower = new Vector2(float.Parse(attribute.Value), float.Parse(xAttribute.Value));
						}
					}
				}

				if (el != null)
				{
					var otherStuff = el.Element("OtherStuff");
					if (otherStuff != null)
					{
						var xAttribute1 = otherStuff.Attribute("InitialMoney");
						if (xAttribute1 != null)
							ls.InitialMoney = int.Parse(xAttribute1.Value);
						var xAttribute = otherStuff.Attribute("MinBananaSpawnTime");
						if (xAttribute != null)
							ls.MinBananaSpawnTime = float.Parse(xAttribute.Value);
						var attribute = otherStuff.Attribute("MaxBananaSpawnTime");
						if (attribute != null)
							ls.MaxBananaSpawnTime = float.Parse(attribute.Value);
					}
				}
			}

			return ls;
		}
	}
}