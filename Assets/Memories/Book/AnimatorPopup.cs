using System.Xml;
using UnityEngine;

namespace Memories.Book;

public class AnimatorPopup : BasePopup
{
    public float leftThreshold = -0.1f;
    public float rightThreshold = 0.1f;

	[SerializeField]
	private string animatorParam = "isFolded";

    private Animator _animator;

	private float lastLift = 0;
	private bool openOnce = true;
	private bool openedOnce = false;

	private void Awake()
	{
        _animator = GetComponent<Animator>();
	}

	public override void DoRotate(float lift)
    {
		bool setvalue = lift < leftThreshold || lift > rightThreshold;
		if ((lift < lastLift)||(openedOnce && openOnce))
		{
			//Debug.Log("AnimatorPopup: Reverse flip detected, settings value to false");
			setvalue = false;
			openedOnce = true;
		}

		_animator.SetBool(animatorParam, !setvalue);
		lastLift = lift;
	}
}
