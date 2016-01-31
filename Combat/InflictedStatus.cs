/// <summary>
/// An inflicted status condition.
/// </summary>
public class InflictedStatus
{
	/// <summary>
	/// Gets the inflicted status.
	/// </summary>
	public Status Status
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the amount of time the status will occur.
	/// </summary>
	public float Duration
	{
		get;
		private set;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="status">The inflicted status.</param>
	/// <param name="duration">The amount of time the status will occur.</param>
	/// <returns>The new object.</returns>
	public InflictedStatus Recreate(Status status, float duration)
	{
		this.Status = status;
		this.Duration = duration;
		return this;
	}
}
