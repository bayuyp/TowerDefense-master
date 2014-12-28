using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeySlow : MonkeyBase
	{
		public new void Start()
		{
			Speed = 3f;
			Range = 100f;
			Damage = 50;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowSlowPooler.GetPooledObject();
		}
	}
}