namespace Assets.Scripts
{
	public class EnemyEasy : EnemyBase
	{
		public new void Start()
		{
			Health = 20;
			Bounty = 5;
			Speed = 1f;
			Damage = 1;
			base.Start();
		}
	}
}