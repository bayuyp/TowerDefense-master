using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class MonkeyUpgraderBase : MonoBehaviour
	{
		public bool IsEnabled;
		private bool isMouseDown;
		public MonkeyBase MonkeyBase;

		public void Start()
		{
			isMouseDown = false;
			IsEnabled = false;
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
			else if (Input.GetMouseButtonUp(0) && isMouseDown && GetComponent<BoxCollider2D>() ==
			         Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("MonkeyUpgrader")))
			{
				Upgrade();
				isMouseDown = false;
			}
			else
				isMouseDown = false;
		}

		public void Show()
		{
			IsEnabled = true;
			//munculkan, kalo duit kurang di transparankan
		}

		public void Hide()
		{
			IsEnabled = false;
		}

		public virtual bool CanUpgrade()
		{
			return IsEnabled;
		}

		public virtual void Upgrade()
		{
			throw new NotImplementedException();
		}
	}
}