using UnityEngine;

/// <summary>
/// base class for all ui state machine behaviors
/// </summary>
public class AnimatorController : StateMachineBehaviour
{
	/// <summary>
	/// Method called by unity when a state is entered
	/// </summary>
	/// <param name="animator">the animator where the script is called</param>
	/// <param name="stateInfo">state info of the new state</param>
	/// <param name="layerIndex">the layer of the state change</param>
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName("Menu_Delete"))
		{
			Destroy(animator.transform.parent.gameObject);
		}
	}
}
