using UnityEngine;

namespace Assets.Scripts
{
	public class Banana : MonoBehaviour
	{
		public float FallSpeed = 1;
		private Camera mainCamera;

		public void Start()
		{
			mainCamera = Camera.main;
		}

		public void Update()
		{
			transform.position = new Vector3(
				transform.position.x,
				transform.position.y - Time.deltaTime*FallSpeed,
				transform.position.z);
			transform.Rotate(0, 0, Time.deltaTime*30);

			if (!Input.GetMouseButtonDown(0))
				return;

			Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

			if (GetComponent<BoxCollider2D>() != Physics2D.OverlapPoint(mousePos,
				1 << LayerMask.NameToLayer("Banana")))
				return;

			GameManager.Instance.AlterMoneyAvailable(Constants.BananaAward);
			Destroy(gameObject);
		}
	}
}