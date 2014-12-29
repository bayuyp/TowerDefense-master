using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyMedium : MonkeyBase
	{
		public new void Start()
		{
			Speed = 1f;
			Range = 4f;
			Damage = 20;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowMediumPooler.GetPooledObject();
		}
	}
}