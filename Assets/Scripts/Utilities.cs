using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utilities
    {
        /// <summary>
        /// Found here
        /// http://www.bensilvis.com/?p=500
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public static void AutoResize(int screenWidth, int screenHeight)
        {
            Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
        }

        /// <summary>
        /// Reads the XML file
        /// </summary>
        /// <returns>A new FileStuffFromXML object</returns>
        public static LevelStuffFromXML ReadXMLFile(int lvl)
        {
			String StringLevel = "";
			if (lvl == 1) StringLevel = "Level1";
			else if (lvl == 2) StringLevel = "Level2";
			else if (lvl == 3) StringLevel = "Level3";
			else if (lvl == 4) StringLevel = "Level4";
			else if (lvl == 5) StringLevel = "Level5";
			else if (lvl == 6) StringLevel = "Level6";
			else if (lvl == 7) StringLevel = "Level7";
			else if (lvl == 8) StringLevel = "Level8";
			else if (lvl == 9) StringLevel = "Level9";
			else if (lvl == 10) StringLevel = "Level10";


            LevelStuffFromXML ls = new LevelStuffFromXML();
            //we're directly loading the level1 file, change if appropriate
            TextAsset ta = Resources.Load(StringLevel) as TextAsset;
            //LINQ to XML rulez!
            XDocument xdoc = XDocument.Parse(ta.text);
            XElement el = xdoc.Element("Elements");
            var paths = el.Element("PathPieces").Elements("Path");

            foreach (var item in paths)
            {
                ls.Paths.Add(new Vector2(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value)));
            }

            var waypoints = el.Element("Waypoints").Elements("Waypoint");
            foreach (var item in waypoints)
            {
                ls.Waypoints.Add(new Vector2(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value)));
            }

            var rounds = el.Element("Rounds").Elements("Round");

			foreach (var item in rounds)
			{
				//int num = int.Parse(item.Attribute("NoOfEnemies").Value);
				//ls.Rounds.Add(new Round(num));


				ls.Rounds.Add(new Round()
				{
						//NoOfEnemies = int.Parse(item.Attribute("NoOfEnemies").Value)
						NoOfEnemiesEasy = int.Parse(item.Attribute("NoOfEnemiesEasy").Value),
						NoOfEnemiesNormal = int.Parse(item.Attribute("NoOfEnemiesNormal").Value),
						NoOfEnemiesHard = int.Parse(item.Attribute("NoOfEnemiesHard").Value),
				});
			}



            XElement tower = el.Element("Tower");
            ls.Tower = new Vector2(float.Parse(tower.Attribute("X").Value), float.Parse(tower.Attribute("Y").Value));

            XElement otherStuff = el.Element("OtherStuff");
            ls.InitialMoney = int.Parse(otherStuff.Attribute("InitialMoney").Value);

            return ls;
        }
    }
}
