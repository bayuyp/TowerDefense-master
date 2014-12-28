namespace Assets.Scripts
{
	public class EnemyHard : EnemyBase
	{
		public new void Start()
		{
			Health = Constants.EnemyHardInitialHealth;
			Bounty = Constants.EnemyHardBounty;
			Speed = Constants.EnemyHardSpeed;
			base.Start();
		}
	}
}