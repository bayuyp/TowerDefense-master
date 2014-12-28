namespace Assets.Scripts
{
	public class EnemyNormal : EnemyBase
	{
		public new void Start()
		{
			Health = 60;
			Bounty = 30;
			Speed = 1.5f;
			base.Start();
		}
	}
}