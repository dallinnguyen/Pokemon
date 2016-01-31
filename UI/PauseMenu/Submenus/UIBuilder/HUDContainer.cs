using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Parent class to all containers in the UI Builder
/// </summary>
public abstract class HUDContainer : Selectable, IPointerClickHandler, ISubmitHandler, IBeginDragHandler
{
	[SerializeField]
	private RectTransform rectTransform;
	private ScreenSpace screenSpace;
	private bool highlighted = false;
	private bool dragged = false;
	private float windowAlpha;
	private HUD hud;

	/// <summary>
	/// A unique identifier for each container, used for storage
	/// </summary>
	public int WindowId { get; set; }

	/// <summary>
	/// Gets: The content of the container (the health bar, stamina bar, etc)
	/// </summary>
	public HUDPrefab Content { get; private set; }

	public RectTransform RectTransform
	{
		get
		{
			return rectTransform;
		}
	}

	protected ScreenSpace ScreenSpace
	{
		get
		{
			return screenSpace;
		}
	}

	protected HUD HudMode
	{
		get
		{
			return hud;
		}
	}

	protected bool Highlighted
	{
		get
		{
			return highlighted;
		}
		set
		{
			highlighted = value;
		}
	}

	public void Initialize(HUDPrefab content, ScreenSpace screenSpace, int windowId, Vector2 anchorMin, Vector2 anchorMax, float windowAlpha, HUD hud)
	{
		this.Content = content;
		this.screenSpace = screenSpace;
		this.WindowId = windowId;
		this.rectTransform.anchorMax = anchorMax;
		this.rectTransform.anchorMin = anchorMin;
		this.windowAlpha = windowAlpha;
		this.hud = hud;
		transform.SetParent(screenSpace.transform, false);
		if (content != null)
		{
			content.ApplyHUDChanges(transform, windowAlpha);
		}
	}

	public void SetContent(HUDPrefab content, int windowId)
	{
		this.Content = content;
		this.WindowId = windowId;
		content.ApplyHUDChanges(transform, windowAlpha);
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

	public void OnBeginDrag(PointerEventData data)
	{
		dragged = true;
		ScreenSpace.PutWindowInFront(WindowId);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(!dragged)
		{
			OnClick();
		}
		else
		{
			dragged = false;
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		OnClick();
	}

	protected virtual void OnClick()
	{
		screenSpace.Callback.WindowID = WindowId;
		UIController.Instance.OpenPopup<int>(Popup.OpenWindowDialog, screenSpace.Callback);
	}
		
	private void Select(bool highlighted)
	{
		if(Content != null)
		{
			if (highlighted)
			{
				Content.WindowAlpha = 1;
			}
			else
			{
				Content.WindowAlpha = windowAlpha;
			}
		}

		this.highlighted = highlighted;
	}
}