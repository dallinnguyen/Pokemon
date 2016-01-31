using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Makes and container drag- and resizeable
/// </summary>
public class ResizeableAndDraggableWindow : HUDContainer, IUsesKeyCode, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private float movementFactor;
	[SerializeField]
	private float scaleFactor;

	public void OnDrag(PointerEventData data)
	{
		Vector2 localPointerPosition;

		RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)ScreenSpace.transform, data.position, data.pressEventCamera, out localPointerPosition);

		RectTransform.localPosition = localPointerPosition;

		ClampToWindow();
	}

	public void OnEndDrag(PointerEventData data)
	{
		AnchorsToCorners();
		Select(false);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		Select(true);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		Select(false);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		Select(true);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		Select(false);
	}

	public void OnKeyCodeDown(int key, float strength, InputLayer layer)
	{
	}

	public void OnKeyCode(int key, float strength, InputLayer layer)
	{
		// Position
		if (Highlighted && key == (int)AxisKey.RSYp && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.y > 0 && Time.deltaTime * movementFactor * strength <= RectTransform.anchorMin.y)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y - (Time.deltaTime * movementFactor * strength));
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y - (Time.deltaTime * movementFactor * strength));
			}
			else if (RectTransform.anchorMin.y > 0)
			{
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y - RectTransform.anchorMin.y);
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, 0);
			}
		}
		else if (Highlighted && key == (int)AxisKey.RSYm && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMax.y < 1 && Time.deltaTime * movementFactor * strength <= 1 - RectTransform.anchorMax.y)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y + (Time.deltaTime * movementFactor * strength));
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y + (Time.deltaTime * movementFactor * strength));
			}
			else if (RectTransform.anchorMax.y < 1)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y + 1 - RectTransform.anchorMax.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, 1);
			}
		}
		if (Highlighted && key == (int)AxisKey.RSXp && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMax.x < 1 && Time.deltaTime * movementFactor * strength <= 1 - RectTransform.anchorMax.x)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x + (Time.deltaTime * movementFactor * strength), RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x + (Time.deltaTime * movementFactor * strength), RectTransform.anchorMax.y);
			}
			else if (RectTransform.anchorMax.x < 1)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x + 1 - RectTransform.anchorMax.x, RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(1, RectTransform.anchorMax.y);
			}
		}
		else if (Highlighted && key == (int)AxisKey.RSXm && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.x > 0 && Time.deltaTime * movementFactor * strength <= RectTransform.anchorMin.x)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x - (Time.deltaTime * movementFactor * strength), RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x - (Time.deltaTime * movementFactor * strength), RectTransform.anchorMax.y);
			}
			else if (RectTransform.anchorMin.x > 0)
			{
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x - RectTransform.anchorMin.x, RectTransform.anchorMax.y);
				RectTransform.anchorMin = new Vector2(0, RectTransform.anchorMin.y);
			}
		}

		// Scale
		if (Highlighted && (key == (int)KeyCode.W || key == (int)AxisKey.HatYp) && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.y > 0 && RectTransform.anchorMax.y < 1 && Time.deltaTime * scaleFactor * strength <= RectTransform.anchorMin.y && Time.deltaTime * scaleFactor * strength <= 1 - RectTransform.anchorMax.y)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y - (Time.deltaTime * scaleFactor * strength));
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y + (Time.deltaTime * scaleFactor * strength));
			}
			else if (RectTransform.anchorMin.y > 0 && Time.deltaTime * scaleFactor * strength <= RectTransform.anchorMin.y)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y - (Time.deltaTime * scaleFactor * strength));
			}
			else if (RectTransform.anchorMax.y < 1 && Time.deltaTime * scaleFactor * strength <= 1 - RectTransform.anchorMax.y)
			{
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y + (Time.deltaTime * scaleFactor * strength));
			}
			else
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, 0);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, 1);
			}
		}
		else if (Highlighted && (key == (int)KeyCode.S || key == (int)AxisKey.HatYm) && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.y + 0.01f < RectTransform.anchorMax.y && Time.deltaTime * scaleFactor * 2 * strength <= RectTransform.anchorMax.y - RectTransform.anchorMin.y)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y + (Time.deltaTime * scaleFactor * strength));
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMax.y - (Time.deltaTime * scaleFactor * strength));
			}
			else
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x, RectTransform.anchorMin.y + 0.01f);
			}
		}
		if (Highlighted && (key == (int)KeyCode.A || key == (int)AxisKey.HatXm) && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.x + 0.01f < RectTransform.anchorMax.x && Time.deltaTime * scaleFactor * 2 * strength <= RectTransform.anchorMax.x - RectTransform.anchorMin.x)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x + (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x - (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMax.y);
			}
			else
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMin.x + 0.01f, RectTransform.anchorMax.y);
			}
		}
		else if (Highlighted && (key == (int)KeyCode.D || key == (int)AxisKey.HatXp) && layer == InputLayer.UI)
		{
			if (RectTransform.anchorMin.x > 0 && RectTransform.anchorMax.x < 1 && Time.deltaTime * scaleFactor * strength <= RectTransform.anchorMin.x && Time.deltaTime * scaleFactor <= 1 - RectTransform.anchorMax.x)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x - (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x + (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMax.y);
			}
			else if (RectTransform.anchorMin.x > 0 && Time.deltaTime * scaleFactor * strength <= RectTransform.anchorMin.x)
			{
				RectTransform.anchorMin = new Vector2(RectTransform.anchorMin.x - (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMin.y);
			}
			else if (RectTransform.anchorMax.x < 1 && Time.deltaTime * scaleFactor * strength <= 1 - RectTransform.anchorMax.x)
			{
				RectTransform.anchorMax = new Vector2(RectTransform.anchorMax.x + (Time.deltaTime * scaleFactor * strength), RectTransform.anchorMax.y);
			}
			else
			{
				RectTransform.anchorMin = new Vector2(0, RectTransform.anchorMin.y);
				RectTransform.anchorMax = new Vector2(1, RectTransform.anchorMax.y);
			}
		}
	}

	protected override void Awake()
	{
		Input.Instance.SubscribeToKeyCode(new int[] { (int)AxisKey.HatXp, (int)AxisKey.HatXm, (int)AxisKey.HatYm, (int)AxisKey.HatYp, (int)KeyCode.W, (int)KeyCode.A, (int)KeyCode.S, (int)KeyCode.D, (int)AxisKey.RSXm, (int)AxisKey.RSXp, (int)AxisKey.RSYm, (int)AxisKey.RSYp, (int)KeyCode.JoystickButton3 }, this);
	}

	/// <summary>
	/// Clamps panel to area of parent
	/// </summary>
	private void ClampToWindow()
	{
		RectTransform parentRectTransform = (RectTransform)ScreenSpace.transform;
		Vector3 pos = RectTransform.localPosition;

		Vector3 minPosition = parentRectTransform.rect.min - RectTransform.rect.min;
		Vector3 maxPosition = parentRectTransform.rect.max - RectTransform.rect.max;

		pos.x = Mathf.Clamp(RectTransform.localPosition.x, minPosition.x, maxPosition.x);
		pos.y = Mathf.Clamp(RectTransform.localPosition.y, minPosition.y, maxPosition.y);

		RectTransform.localPosition = pos;
	}

	private void Select(bool select)
	{
		if (select)
		{
			Content.WindowAlpha = 1;
		}
		else
		{
			Content.WindowAlpha = 0.2f;
		}
		Highlighted = select;
	}

	private void AnchorsToCorners()
	{
		RectTransform t = RectTransform;
		RectTransform pt = (RectTransform)ScreenSpace.transform;

		Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + (t.offsetMin.x / pt.rect.width), t.anchorMin.y + (t.offsetMin.y / pt.rect.height));
		Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + (t.offsetMax.x / pt.rect.width), t.anchorMax.y + (t.offsetMax.y / pt.rect.height));

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}
}