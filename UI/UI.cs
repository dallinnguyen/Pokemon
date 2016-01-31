using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// References for other classes
/// </summary>
public class UI : PPNonPersistantBehaviourSingleton<UI>, IInitializeable
{
	[SerializeField]
	private EventSystem eventSystem;
	[SerializeField]
	private RectTransform hudParent;
	[SerializeField]
	private Canvas canvas;

	public EventSystem EventSystem
	{
		get
		{
			return eventSystem;
		}
	}

	public RectTransform HUDParent
	{
		get
		{
			return hudParent;
		}
	}

	public Canvas Canvas 
	{
		get
		{
			return canvas;
		}
	}

	protected override UI GetInstance()
	{
		return this;
	}

	protected override void Initialize()
	{
	}
}