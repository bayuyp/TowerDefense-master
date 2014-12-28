using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeySlow : MonkeyBase
	{
		public new void Start()
		{
			ShootWaitTime = Constants.MonkeySlowSpeed;
			base.Start();
		}

		protected override GameObject GetPooledArrow()
		{
			return ObjectPoolerManager.Instance.ArrowSlowPooler.GetPooledObject();
		}
	}
}