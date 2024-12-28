using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private Vector3 _startPos;

        public Transform offShelfPosition;
        public Transform readingPosition;

        public Transform cameraTransform;

        public float pageSeparation;

        public bool open;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _startPos = transform.position;
        }

        private void Update()
        {
            _animator.SetBool(_openProp, open);

            float absDiff = Mathf.Abs(left.transform.localEulerAngles.y - right.transform.localEulerAngles.y);
            float degreeDiff = absDiff % 360;
            const float oneOver180 = 1f / 180f; // multiplication is faster than division;
            pageSeparation = degreeDiff * oneOver180;

            if (UnityEngine.Input.GetKeyDown(KeyCode.A)) TakeOut().Forget();
        }

        private async UniTask TakeOut()
        {
            await transform.LerpTransform(offShelfPosition, 0.3f);
            transform.LerpTransform(readingPosition, 1f).Forget();

            await UniTask.Delay(700);
            cameraTransform.DOLocalRotate(new Vector3(70, 0, 0), 0.5f);
        }
    }
}
