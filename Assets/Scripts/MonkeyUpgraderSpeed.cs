namespace Assets.Scripts
{
	public class MonkeyUpgraderSpeed : MonkeyUpgraderBase
	{
		public override void Upgrade()
		{
			GameManager.Instance.AlterMoneyAvailable(MonkeyBase.UpgradeCostSpeed);
			MonkeyBase.Speed -= MonkeyBase.Speed*50/100;
			MonkeyBase.LevelSpeed++;
		}

		public override bool CanUpgrade()
		{
			return base.CanUpgrade() && MonkeyBase.UpgradeCostSpeed <= GameManager.Instance.MoneyAvailable;
		}
	}
}