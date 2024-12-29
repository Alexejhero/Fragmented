using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Book")]
    public class MemoryBook : MonoBehaviour
    {
        private static readonly int _openProp = Animator.StringToHash("open");
        private static readonly int _finishedProp = Animator.StringToHash("finished");

        private Animator _animator;

        private Vector3 _startPos;

        private Vector3 _cameraStartPos;
        private Vector3 _cameraStartRot;

        public Transform offShelfPosition;
        [FormerlySerializedAs("readingPosition")] public Transform previewPosition;

        public Transform cameraTransform;
        public Transform cameraPreviewLocation;
        public Transform cameraReadingLocation;
        public GameObject bookshelfObject;

        [HideInInspector]
        public bool inTransition;

        [HideInInspector]
        public float pageSeparation;

        public bool open;
        public bool finished;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _startPos = transform.position;

            _cameraStartPos = cameraTransform.position;
            _cameraStartRot = cameraTransform.eulerAngles;
        }

        private void Update()
        {
            _animator.SetBool(_openProp, open);
            _animator.SetBool(_finishedProp, finished);

            // float absDiff = Mathf.Abs(left.transform.localEulerAngles.y - right.transform.localEulerAngles.y);
            // float degreeDiff = absDiff % 360;
            // const float oneOver180 = 1f / 180f; // multiplication is faster than division;
            // pageSeparation = degreeDiff * oneOver180;

            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) TakeOut().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.O)) Open().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.C)) Close().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) Stuff().Forget();
        }

        public async UniTask Stuff()
        {
            while (true)
            {
                await Open();
                await UniTask.Delay(2000);
                await Close();
                await UniTask.Delay(2000);
            }
        }

        public async UniTask TakeOut()
        {
            inTransition = true;
            await transform.LerpTransform(offShelfPosition, 0.3f);
            transform.LerpTransform(previewPosition, 1f).Forget();

            await UniTask.Delay(700);
            cameraTransform.DOMove(cameraPreviewLocation.position, 0.5f);
            cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.5f);
            inTransition = false;
        }

        private async UniTask Open()
        {
            bookshelfObject.SetActive(false);
            open = true;
            cameraTransform.DOMove(cameraReadingLocation.position, 0.85f);
            cameraTransform.DORotate(cameraReadingLocation.eulerAngles, 0.85f);
        }

        private async UniTask Close()
        {
            open = false;
            await UniTask.Delay(500);
            cameraTransform.DOMove(cameraPreviewLocation.position, 0.7f);
            cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.7f);
            await UniTask.Delay(700);
            bookshelfObject.SetActive(true);
        }
    }
}
