namespace Assets.Scripts
{
	public class EnemyEasy : EnemyBase
	{
		public new void Start()
		{
			Health = 20;
			Bounty = 20;
			Speed = 3f;
			Damage = 1;
			base.Start();
		}
	}
}