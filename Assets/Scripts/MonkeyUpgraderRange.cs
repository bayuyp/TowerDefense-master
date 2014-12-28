namespace Assets.Scripts
{
	public class MonkeyUpgraderRange : MonkeyUpgraderBase
	{
		public override void Upgrade()
		{
			GameManager.Instance.AlterMoneyAvailable(MonkeyBase.UpgradeCostRange);
			MonkeyBase.Range += MonkeyBase.Range*20/100;
			MonkeyBase.LevelRange++;
		}

		public override bool CanUpgrade()
		{
			return MonkeyBase.GetComponent<MonkeyBase>().UpgradeCostRange <= GameManager.Instance.MoneyAvailable &&
			       base.CanUpgrade();
		}
	}
}