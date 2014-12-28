using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyUpgraderBase : MonoBehaviour
	{
		private bool isEnabled;
		private bool isMouseDown;
		public MonkeyBase MonkeyBase;

		public void Start()
		{
			isMouseDown = false;
			Hide();
		}

		public void Update()
		{
			if (!CanUpgrade())
				return;

			var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Input.GetMouseButtonDown(0) &&
			    GetComponent<BoxCollider2D>() ==
			    Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("MonkeyUpgrader")))
				isMouseDown = true;
			else if (Input.GetMouseButtonUp(0))
			{
				if (isMouseDown && GetComponent<BoxCollider2D>() ==
				    Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("MonkeyUpgrader")))
				{
					Upgrade();
				}
				isMouseDown = false;
			}
		}

		public void Show()
		{
			isEnabled = true;
			var temp = renderer.material.color;
			temp.a = 1f;
			renderer.material.color = temp;
			//munculkan, kalo duit kurang di transparankan
		}

		public void Hide()
		{
			isEnabled = false;
			var temp = renderer.material.color;
			temp.a = 0f;
			renderer.material.color = temp;
		}

		public virtual bool CanUpgrade()
		{
			return MonkeyBase != null && isEnabled;
		}

		public virtual void Upgrade()
		{
			throw new NotImplementedException();
		}
	}
}