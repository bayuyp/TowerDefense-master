using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Constant helper variables
    /// </summary>
    public static class Constants
    {
        public static readonly Color RedColor = new Color(1f, 0f, 0f, 0f);
        public static readonly Color BlackColor = new Color(0f, 0f, 0f, 0f);

        public static readonly int BunnyCost = 50;

		public static readonly float MinDistanceForBunnyToShoot = 3f;
        public static readonly int CarrotAward = 10;

        public static readonly int InitialEnemyEasyHealth = 50;
		public static readonly int InitialEnemyNormalHealth = 75;
		public static readonly int InitialEnemyHardHealth = 100;

        public static readonly int ArrowDamage = 25;
		public static readonly int ArrowFastDamage = 10;
		public static readonly int ArrowSlowDamage = 50;    

		public static readonly int EasyEnemyBounty = 20;
		public static readonly int NormalEnemyBounty = 30;
		public static readonly int HardEnemyBounty = 45;

		public static readonly float SpeedyEnemyEasy = 1f;
		public static readonly float SpeedyEnemyNormal = 1.5f;
		public static readonly float SpeedyEnemyHard = 2f;
    }
}
