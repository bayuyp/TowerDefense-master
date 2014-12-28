using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyFast : MonkeyBase
	{
		public new void Start()
		{
			Speed = 0.3f;
			Range = 100f;
			Damage = 3;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowFastPooler.GetPooledObject();
		}
	}
}