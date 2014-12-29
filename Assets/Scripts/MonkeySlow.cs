using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeySlow : MonkeyBase
	{
		public new void Start()
		{
			Speed = 3f;
			Range = 5f;
			Damage = 80;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowSlowPooler.GetPooledObject();
		}
	}
}