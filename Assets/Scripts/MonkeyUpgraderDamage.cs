namespace Assets.Scripts
{
	public class MonkeyUpgraderDamage : MonkeyUpgraderBase
	{
		public override void Upgrade()
		{
			GameManager.Instance.AlterMoneyAvailable(MonkeyBase.UpgradeCostDamage);
			MonkeyBase.Damage += MonkeyBase.Damage*50/100;
			MonkeyBase.LevelDamage++;
		}

		public override bool CanUpgrade()
		{
			return base.CanUpgrade() && MonkeyBase.UpgradeCostDamage <= GameManager.Instance.MoneyAvailable;
		}
	}
}