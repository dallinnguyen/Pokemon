using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// base class for all timers
/// </summary>
public abstract class Timer : Text
{
	private float timer, startValue;
	private WaitForEndOfFrame wait = new WaitForEndOfFrame();

	/// <summary>
	/// Gets: The WaitFotEndOfFrame
	/// </summary>
	protected WaitForEndOfFrame Wait
	{
		get
		{
			return wait;
		}
	}

	/// <summary>
	/// Starts the timer
	/// </summary>
	public void OnActivated()
	{
		StartCoroutine(DecreaseTimer());
	}

	/// <summary>
	/// Resets the timer
	/// </summary>
	public void ResetTimer()
	{
		timer = startValue;
		text = string.Empty + ((int)timer - 1);
	}

	/// <summary>
	/// Called after end of the timer
	/// </summary>
	protected abstract void OnFinished();

	private new void Awake()
	{
		timer = startValue = float.Parse(text) + 1;
	}

	private IEnumerator DecreaseTimer()
	{
		while (timer > 0)
		{
			yield return wait;
			timer -= Time.deltaTime;
			text = string.Empty + (int)timer;
		}
		OnFinished();
	}
}