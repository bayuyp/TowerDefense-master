using UnityEngine;

namespace Assets.Scripts
{
	public class Arrow : MonoBehaviour
	{
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