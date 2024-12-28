using Helpers;
using Memories.Characters.Movement;
using TriInspector;
using UnityEngine;

namespace Memories.Characters
{
    /// <summary>
    /// For now I'll use this just to test crude movement and animations. Planning to sync with Govo to reuse our old controller
    /// </summary>
    public sealed class CharacterAnimator : MonoBehaviour
    {
        private static readonly int _isLeft = Animator.StringToHash("isLeft");
        private static readonly int _isMoving = Animator.StringToHash("isMoving");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private PlayerController controller;

        [ShowInInspector]
        private bool isMoving;
        [ShowInInspector]
        private bool isLeft = true;
        private void Awake()
        {
            this.EnsureComponent(ref animator);
            this.EnsureComponent(ref rb);
            this.EnsureComponent(ref controller);
        }

        private void Update()
        {
            isMoving = controller.MoveInput.magnitude > 0.01f;
            // move input overrides velocity
            if (Mathf.Abs(controller.MoveInput.x) > 0.01f)
            {
                isLeft = controller.MoveInput.x < -0.01f;
            }
            // else if (Mathf.Abs(rb.velocity.x) > 0.1f)
            // {
            //     isLeft = rb.velocity.x < -0.1f;
            // }

            // Debug.Log(isMoving ? $"moving {(isLeft ? "left" : "right")}" : "standing");
            animator.SetBool(_isLeft, isLeft);
            animator.SetBool(_isMoving, isMoving);

            transform.localScale = isLeft ? new Vector3(-1, 1, 1) : Vector3.one;
        }

        private void PlayerReset()
        {
            animator.SetBool(_isLeft, true);
            animator.SetBool(_isMoving, false);
        }
    }
}
