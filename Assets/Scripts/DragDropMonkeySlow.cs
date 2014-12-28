namespace Assets.Scripts
{
	public class DragDropMonkeySlow : DragDropMonkeyBase
	{
		public new void Start()
		{
			MonkeyCost = Constants.MonkeySlowCost;
			base.Start();
		}
	}
}