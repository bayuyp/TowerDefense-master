namespace Assets.Scripts
{
	public class DragDropMonkeyFast : DragDropMonkeyBase
	{
		public new void Start()
		{
			MonkeyCost = Constants.MonkeyFastCost;
			base.Start();
		}
	}
}