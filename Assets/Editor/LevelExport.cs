using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	public class LevelExport : EditorWindow
	{
		private readonly List<Round> rounds = new List<Round>();
		private XDocument doc;
		private string filename = "LevelX.xml";
		private int initialMoney;
		private int noOfEnemiesEasy;
		private int noOfEnemiesHard;
		private int noOfEnemiesNormal;
		private int pathsCount;
		private Vector2 scrollPosition = Vector2.zero;
		private int waypointsCount;

		[MenuItem("Custom Editor/Export Level")]
		public static void ShowWindow()
		{
			GetWindow(typeof (LevelExport));
		}

		public void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			EditorGUILayout.LabelField("Total Rounds created:" + rounds.Count);
			for (var i = 0; i < rounds.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Round " + (i + 1));
				EditorGUILayout.LabelField("Easy:" + rounds[i].NoOfEnemiesEasy);
				EditorGUILayout.LabelField("Normal:" + rounds[i].NoOfEnemiesNormal);
				EditorGUILayout.LabelField("Hard:" + rounds[i].NoOfEnemiesHard);
				if (GUILayout.Button("Delete"))
					rounds.RemoveAt(i);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.LabelField("Add a new round", EditorStyles.boldLabel);
			noOfEnemiesEasy = EditorGUILayout.IntSlider("Enemies Easy", noOfEnemiesEasy, 1, 20);
			noOfEnemiesNormal = EditorGUILayout.IntSlider("Enemies Normal", noOfEnemiesNormal, 1, 20);
			noOfEnemiesHard = EditorGUILayout.IntSlider("Enemies Hard", noOfEnemiesHard, 1, 20);

			if (GUILayout.Button("Add new round"))
				rounds.Add(new Round
				{
					NoOfEnemiesEasy = noOfEnemiesEasy,
					NoOfEnemiesNormal = noOfEnemiesNormal,
					NoOfEnemiesHard = noOfEnemiesHard
				});

			initialMoney = EditorGUILayout.IntSlider("Initial Money", initialMoney, 100, 500);
			//MinBananaSpawnTime = EditorGUILayout.IntSlider("MinBananaSpawnTime", MinBananaSpawnTime, 1, 10);
			//MaxBananaSpawnTime = EditorGUILayout.IntSlider("MaxBananaSpawnTime", MaxBananaSpawnTime, 1, 10);

			filename = EditorGUILayout.TextField("Filename:", filename);
			EditorGUILayout.LabelField("Export Level", EditorStyles.boldLabel);
			if (GUILayout.Button("Export"))
				Export();
		}

		// The export method
		private void Export()
		{
			// Create a new output file stream
			doc = new XDocument();
			doc.Add(new XElement("Elements"));
			var elements = doc.Element("Elements");

			var pathPiecesXML = new XElement("PathPieces");
			var paths = GameObject.FindGameObjectsWithTag("Path");

			foreach (var item in paths)
			{
				var path = new XElement("Path");
				var attrX = new XAttribute("X", item.transform.position.x);
				var attrY = new XAttribute("Y", item.transform.position.y);
				path.Add(attrX, attrY);
				pathPiecesXML.Add(path);
			}
			pathsCount = paths.Length;
			if (elements != null)
			{
				elements.Add(pathPiecesXML);

				var waypointsXML = new XElement("Waypoints");
				var waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
				if (!WaypointsAreValid(waypoints))
					return;
				//order by user selected order
				waypoints = waypoints.OrderBy(x => x.GetComponent<OrderedWaypointForEditor>().Order).ToArray();
				foreach (var item in waypoints)
				{
					var waypoint = new XElement("Waypoint");
					var attrX = new XAttribute("X", item.transform.position.x);
					var attrY = new XAttribute("Y", item.transform.position.y);
					waypoint.Add(attrX, attrY);
					waypointsXML.Add(waypoint);
				}
				waypointsCount = waypoints.Length;
				elements.Add(waypointsXML);

				var roundsXML = new XElement("Rounds");
				foreach (var item in rounds)
				{
					var round = new XElement("Round");
					var NoOfEnemiesEasy = new XAttribute("NoOfEnemiesEasy", item.NoOfEnemiesEasy);
					var NoOfEnemiesNormal = new XAttribute("NoOfEnemiesNormal", item.NoOfEnemiesNormal);
					var NoOfEnemiesHard = new XAttribute("NoOfEnemiesHard", item.NoOfEnemiesHard);
					round.Add(NoOfEnemiesEasy);
					round.Add(NoOfEnemiesNormal);
					round.Add(NoOfEnemiesHard);
					roundsXML.Add(round);
				}
				elements.Add(roundsXML);

				var towerXML = new XElement("Tower");
				var tower = GameObject.FindGameObjectWithTag("Tower");
				if (tower == null)
				{
					ShowErrorForNull("Tower");
					return;
				}
				var towerX = new XAttribute("X", tower.transform.position.x);
				var towerY = new XAttribute("Y", tower.transform.position.y);
				towerXML.Add(towerX, towerY);
				elements.Add(towerXML);

				var otherStuffXML = new XElement("OtherStuff");
				otherStuffXML.Add(new XAttribute("InitialMoney", initialMoney));
				elements.Add(otherStuffXML);
			}

			if (!InputIsValid())
				return;

			if (EditorUtility.DisplayDialog("Save confirmation",
				"Are you sure you want to save level " + filename + "?", "OK", "Cancel"))
			{
				doc.Save("Assets/" + filename);
				EditorUtility.DisplayDialog("Saved", filename + " saved!", "OK");
			}
			else
				EditorUtility.DisplayDialog("NOT Saved", filename + " not saved!", "OK");
		}

		private bool WaypointsAreValid(GameObject[] waypoints)
		{
			//first check whether whey all have a OrderedWaypoint component
			if (waypoints.Any(x => x.GetComponent<OrderedWaypointForEditor>() == null))
			{
				EditorUtility.DisplayDialog("Error", "All waypoints must have an ordered waypoint component", "OK");
				return false;
			}
			//check if all Order fields on the orderwaypoint components are different

			if (waypoints.Count() != waypoints.Select(x => x.GetComponent<OrderedWaypointForEditor>().Order).Distinct().Count())
			{
				EditorUtility.DisplayDialog("Error", "All waypoints must have a different order", "OK");
				return false;
			}
			return true;
		}

		private void ShowErrorForNull(string gameObjectName)
		{
			EditorUtility.DisplayDialog("Error", "Cannot find gameobject " + gameObjectName, "OK");
		}

		private bool InputIsValid()
		{
			if (rounds.Count == 0)
			{
				EditorUtility.DisplayDialog("Error", "You cannot have 0 rounds", "OK");
				return false;
			}

			if (waypointsCount == 0)
			{
				EditorUtility.DisplayDialog("Error", "You cannot have 0 waypoints", "OK");
				return false;
			}

			if (pathsCount == 0)
			{
				EditorUtility.DisplayDialog("Error", "You cannot have 0 paths", "OK");
				return false;
			}

			return true;
		}
	}
}