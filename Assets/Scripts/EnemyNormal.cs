namespace Assets.Scripts
{
	public class EnemyNormal : EnemyBase
	{
		public new void Start()
		{
			Health = 60;
			Bounty = 10;
			Speed = 1.5f;
			Damage = 2;
			base.Start();
		}
	}
}