/// <summary>
/// Holds individual Pokemon information.
/// </summary>
public class Pokemon
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Pokemon" /> class.
	/// </summary>
    public Pokemon()
    {
        CurrentHealth = MaxHealth = 100;
        CurrentStamina = MaxStamina = 100;
    }

	/// <summary>
	/// Gets or sets the maximum health of the Pokemon.
	/// </summary>
	public int MaxHealth
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the current health of the Pokemon.
	/// </summary>
	public int CurrentHealth
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the maximum stamina of the Pokemon.
	/// </summary>
	public int MaxStamina
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the current stamina of the Pokemon.
	/// </summary>
	public int CurrentStamina
	{
		get;
		set;
	}
}
