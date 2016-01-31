using System;

/// <summary>
/// The interface of all callback classes
/// </summary>
/// <typeparam name="T">the type of the argument</typeparam>
public interface ICallback<T>
{
	/// <summary>
	/// The callback method
	/// </summary>
	/// <param name="arg">the argument of the callback</param>
	void Method(T arg);
}