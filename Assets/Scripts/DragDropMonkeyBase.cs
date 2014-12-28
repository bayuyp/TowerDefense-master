using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class DragDropMonkeyBase : MonoBehaviour
	{
		private bool isDragging;
		private Camera mainCamera;
		protected int MonkeyCost;
		public GameObject MonkeyGenerator;
		public GameObject MonkeyPrefab;
		private GameObject tempBackgroundBehindPath;
		private GameObject tempMonkey;

		public void Start()
		{
			mainCamera = Camera.main;
			isDragging = false;
		}

		public void Update()
		{
			if (Input.GetMouseButtonDown(0) && !isDragging &&
			    GameManager.Instance.MoneyAvailable >= MonkeyCost)
			{
				ResetTempBackgroundColor();
				Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

				if (MonkeyGenerator.GetComponent<CircleCollider2D>() !=
				    Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("MonkeyGenerator")))
					return;

				isDragging = true;
				tempMonkey = Instantiate(MonkeyPrefab, MonkeyGenerator.transform.position, Quaternion.identity)
					as GameObject;
			}
			else if (Input.GetMouseButton(0) && isDragging)
			{
				var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				var hits = Physics2D.RaycastAll(ray.origin, ray.direction);
				if (hits.Length <= 0 || hits[0].collider == null)
					return;

				tempMonkey.transform.position = hits[0].collider.gameObject.transform.position;


				if (hits.Where(x => x.collider.gameObject.tag == "Path"
				                    || x.collider.gameObject.tag == "Tower").Any()
				    || hits.Where(x => x.collider.gameObject.tag == "Monkey").Count() > 1)
				{
					var backgroundBehindPath = hits.Where
						(x => x.collider.gameObject.tag == "Background").First().collider.gameObject;

					backgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.RedColor;

					if (tempBackgroundBehindPath != backgroundBehindPath)
						ResetTempBackgroundColor();

					tempBackgroundBehindPath = backgroundBehindPath;
				}
				else
					ResetTempBackgroundColor();
			}
			//we're stopping dragging
			else if (Input.GetMouseButtonUp(0) && isDragging)
			{
				ResetTempBackgroundColor();
				//check if we can leave the bunny here
				var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				var hits = Physics2D.RaycastAll(ray.origin, ray.direction,
					Mathf.Infinity, ~(1 << LayerMask.NameToLayer("MonkeyGenerator")));
				//in order to place it, we must have a background and no other bunnies
				if (hits.Where(x => x.collider.gameObject.tag == "Background").Any()
				    && !hits.Where(x => x.collider.gameObject.tag == "Path").Any()
				    && hits.Where(x => x.collider.gameObject.tag == "Monkey").Count() == 1)
				{
					//we can leave a bunny here, so decrease money and activate it
					GameManager.Instance.AlterMoneyAvailable(-MonkeyCost);
					tempMonkey.transform.position =
						hits.Where(x => x.collider.gameObject.tag == "Background")
							.First().collider.gameObject.transform.position;
					tempMonkey.GetComponent<MonkeyBase>().Activate();
					GameManager.Instance.Monkeys.Add(tempMonkey);
				}
				else
				//we can't leave a bunny here, so destroy the temp one
					Destroy(tempMonkey);
				isDragging = false;
			}
		}

		private void ResetTempBackgroundColor()
		{
			if (tempBackgroundBehindPath != null)
				tempBackgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.BlackColor;
		}
	}
}