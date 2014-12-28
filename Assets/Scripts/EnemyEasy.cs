namespace Assets.Scripts
{
	public class EnemyEasy : EnemyBase
	{
		public new void Start()
		{
			Health = Constants.EnemyEasyInitialHealth;
			Bounty = Constants.EnemyEasyBounty;
			Speed = Constants.EnemyEasySpeed;
			base.Start();
		}
	}
}