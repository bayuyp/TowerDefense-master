using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyFast : MonkeyBase
	{
		public new void Start()
		{
			Speed = 0.3f;
			Range = 3f;
			Damage = 5;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowFastPooler.GetPooledObject();
		}
	}
}