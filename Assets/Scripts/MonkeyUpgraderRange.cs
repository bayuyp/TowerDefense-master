namespace Assets.Scripts
{
	public class MonkeyUpgraderRange : MonkeyUpgraderBase
	{
		public override void Upgrade()
		{
			GameManager.Instance.AlterMoneyAvailable(MonkeyBase.UpgradeCostRange);
			MonkeyBase.Range += MonkeyBase.Range*50/100;
			MonkeyBase.LevelRange++;
		}

		public override bool CanUpgrade()
		{
			return base.CanUpgrade() && MonkeyBase.UpgradeCostRange <= GameManager.Instance.MoneyAvailable;
		}
	}
}