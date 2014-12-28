namespace Assets.Scripts
{
	public class MonkeyMedium : MonkeyBase
	{
		public new void Start()
		{
			ShootWaitTime = Constants.MonkeyMediumSpeed;
			base.Start();
		}
	}
}