using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Memories.Characters;
using Memories.Cutscenes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Book")]
    public class MemoryBook : MonoBehaviour
    {
        private static readonly int _openProp = Animator.StringToHash("open");
        private static readonly int _pageProp = Animator.StringToHash("page");

        private Animator _animator;

        private Vector3 _startPos;
        private Vector3 _startRot;

        private Vector3 _cameraStartPos;
        private Vector3 _cameraStartRot;

        public Transform offShelfPosition;
        [FormerlySerializedAs("readingPosition")] public Transform previewPosition;

        public Transform cameraTransform;
        public Transform cameraPreviewLocation;
        public Transform cameraReadingLocation;
        public GameObject bookshelfObject;

        public BookActor[] actors;
        public CustomSequencer[] customSequences;

        [HideInInspector]
        public float pageSeparation;

        public bool open;
        public int page;

        public float pageSpreadProgress;

        [SerializeField]
        private State state = State.OnShelf;

        private enum State
        {
            Moving = -1,
            OnShelf,
            Previewing,
            Opened
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _startPos = transform.position;
            _startRot = transform.eulerAngles;

            if (cameraTransform)
            {
                _cameraStartPos = cameraTransform.position;
                _cameraStartRot = cameraTransform.eulerAngles;
            }
        }

        private void Update()
        {
            _animator.SetBool(_openProp, open);
            _animator.SetInteger(_pageProp, page);

            if (UnityEngine.Input.GetKeyDown(KeyCode.E) && state == State.OnShelf) TakeOut().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) && state == State.Previewing) PutBack().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.O) && state == State.Previewing) Open().Forget();
            if (UnityEngine.Input.GetKeyDown(KeyCode.O) && state == State.Opened) Close().Forget();
        }

        public async UniTask TakeOut()
        {
            state = State.Moving;

            await transform.LerpTransform(offShelfPosition, 0.3f);
            transform.LerpTransform(previewPosition, 1f).Forget();

            await UniTask.Delay(700);
            cameraTransform.DOMove(cameraPreviewLocation.position, 0.5f);
            cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.5f);
            await UniTask.Delay(500);

            state = State.Previewing;
        }

        public async UniTask PutBack()
        {
            if (!offShelfPosition) return;

            state = State.Moving;

            cameraTransform.DOMove(_cameraStartPos, 0.5f);
            cameraTransform.DORotate(_cameraStartRot, 0.5f);

            transform.DOMove(offShelfPosition.position, 1);
            transform.DORotate(offShelfPosition.eulerAngles, 1);
            await UniTask.Delay(1000);

            transform.DOMove(_startPos, 0.3f);
            transform.DORotate(_startRot, 0.3f);
            await UniTask.Delay(300);

            state = State.OnShelf;
        }

        private async UniTask Open()
        {
            state = State.Moving;

            if (bookshelfObject) bookshelfObject.SetActive(false);
            open = true;

            if (cameraTransform)
            {
                cameraTransform.DOMove(cameraReadingLocation.position, 0.85f);
                cameraTransform.DORotate(cameraReadingLocation.eulerAngles, 0.85f);
            }

            await UniTask.Delay(2500);

            state = State.Opened;
        }

        private async UniTask Close()
        {
            state = State.Moving;

            open = false;
            await UniTask.Delay(500);

            if (cameraTransform)
            {
                cameraTransform.DOMove(cameraPreviewLocation.position, 0.7f);
                cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.7f);
            }

            await UniTask.Delay(700);
            if (bookshelfObject) bookshelfObject.SetActive(true);
            await UniTask.Delay(2500 - 500 - 700);

            state = State.Previewing;
        }

        public CustomSequencer GetSequencer(string sequenceName)
        {
            return customSequences.FirstOrDefault(s => s.sequenceName == sequenceName);
        }
    }
}
