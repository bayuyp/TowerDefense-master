﻿namespace Assets.Scripts
{
	public class EnemyHard : EnemyBase
	{
		public new void Start()
		{
			Health = 100;
			Bounty = 25;
			Speed = 2f;
			Damage = 3;
			base.Start();
		}
	}
}