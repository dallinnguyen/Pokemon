/// <summary>
/// A range of integers capped by a minimum and maximum value. Object pooled.
/// </summary>
public class IntRange
{
	/// <summary>
	/// Gets the minimum endpoint of the range.
	/// </summary>
	public int Minimum
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the maximum endpoint of the range.
	/// </summary>
	public int Maximum
	{
		get;
		private set;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="minimum">The minimum endpoint of the range.</param>
	/// <param name="maximum">The maximum endpoint of the range.</param>
	public void Recreate(int minimum, int maximum)
	{
		this.Minimum = minimum;
		this.Maximum = maximum;
	}
}
