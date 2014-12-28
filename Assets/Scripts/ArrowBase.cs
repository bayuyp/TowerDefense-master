using UnityEngine;

namespace Assets.Scripts
{
	public class ArrowBase : MonoBehaviour
	{
		public int ArrowDamage;

		public void Disable()
		{
			CancelInvoke();
			gameObject.SetActive(false);
		}

		public void Start()
		{
			Invoke("Disable", 5f);
		}
	}
}