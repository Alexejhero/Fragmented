using UnityEngine;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Book")]
    public class MemoryBook : MonoBehaviour
    {
        private static readonly int _openProp = Animator.StringToHash("open");

        private Animator _animator;

        public GameObject left;
        public GameObject right;

        public float pageSeparation;

        public bool open;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(_openProp, open);

            float absDiff = Mathf.Abs(left.transform.localEulerAngles.y - right.transform.localEulerAngles.y);
            float degreeDiff = absDiff % 360;
            const float ONE_OVER_180 = 1f / 180f; // multiplication is faster than division;
            pageSeparation = degreeDiff * ONE_OVER_180;
        }
    }
}
