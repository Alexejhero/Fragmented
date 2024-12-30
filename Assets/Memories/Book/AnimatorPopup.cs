using UnityEngine;

namespace Memories.Book;

public class AnimatorPopup : BasePopup
{
    public float leftThreshold = -0.1f;
    public float rightThreshold = 0.1f;

	[SerializeField]
	private string animatorParam = "isFolded";

    private Animator _animator;

	private void Awake()
	{
        _animator = GetComponent<Animator>();
	}

	public override void DoRotate(float lift)
    {
		bool setvalue = lift < leftThreshold || lift > rightThreshold;
		_animator.SetBool(animatorParam, !setvalue);
    }
}
