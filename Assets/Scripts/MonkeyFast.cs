namespace Assets.Scripts
{
	public class MonkeyFast : MonkeyBase
	{
		public new void Start()
		{
			ShootWaitTime = Constants.MonkeyFastSpeed;
			base.Start();
		}
	}
}