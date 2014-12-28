namespace Assets.Scripts
{
	public class EnemyNormal : EnemyBase
	{
		public new void Start()
		{
			Health = Constants.EnemyNormalInitialHealth;
			Bounty = Constants.EnemyNormalBounty;
			Speed = Constants.EnemyNormalSpeed;
			base.Start();
		}
	}
}