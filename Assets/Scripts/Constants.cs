using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	///     Constant helper variables
	/// </summary>
	public static class Constants
	{
		public static readonly Color RedColor = new Color(1f, 0f, 0f, 0f);
		public static readonly Color BlackColor = new Color(0f, 0f, 0f, 0f);
		public static readonly int MonkeyMediumCost = 50;
		public static readonly int MonkeyFastCost = 75;
		public static readonly int MonkeySlowCost = 100;
		public static readonly float MonkeyMediumSpeed = 1f;
		public static readonly float MonkeyFastSpeed = 0.3f;
		public static readonly float MonkeySlowSpeed = 3f;
		public static readonly float MinDistanceForMonkeyToShoot = 3f;
		public static readonly int BananaAward = 10;
		public static readonly int EnemyEasyInitialHealth = 1;
		public static readonly int EnemyNormalInitialHealth = 75;
		public static readonly int EnemyHardInitialHealth = 100;
		public static readonly int ArrowMediumDamage = 25;
		public static readonly int ArrowFastDamage = 10;
		public static readonly int ArrowSlowDamage = 50;
		public static readonly int EnemyEasyBounty = 20;
		public static readonly int EnemyNormalBounty = 30;
		public static readonly int EnemyHardBounty = 45;
		public static readonly float EnemyEasySpeed = 1f;
		public static readonly float EnemyNormalSpeed = 1.5f;
		public static readonly float EnemyHardSpeed = 2f;
	}
}