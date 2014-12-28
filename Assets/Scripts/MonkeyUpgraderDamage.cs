namespace Assets.Scripts
{
	public class MonkeyUpgraderDamage : MonkeyUpgraderBase
	{
		public override void Upgrade()
		{
			GameManager.Instance.AlterMoneyAvailable(MonkeyBase.UpgradeCostDamage);
			MonkeyBase.Damage += MonkeyBase.Damage*20/100;
			MonkeyBase.LevelDamage++;
		}

		public override bool CanUpgrade()
		{
			return MonkeyBase.GetComponent<MonkeyBase>().UpgradeCostDamage <= GameManager.Instance.MoneyAvailable &&
			       base.CanUpgrade();
		}
	}
}