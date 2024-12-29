using System;
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

        [FormerlySerializedAs("memory")]
        public MemoryProgression memoryProgression;
        public BookSpread[] pageSpreads;

        public GameObject fakeCover;
        public GameObject realCover;
        public GameObject realArmature;

        [HideInInspector]
        public float pageSeparation;

        [HideInInspector]
        public bool open;

        [HideInInspector]
        public int page;

        [NonSerialized]
        public bool Advancing = true;

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

            if (fakeCover && realCover && realArmature && state == State.OnShelf)
            {
                fakeCover.SetActive(true);
                realCover.SetActive(false);
                realArmature.SetActive(false);
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

            if (UnityEngine.Input.GetKeyDown(KeyCode.C) && state == State.Opened)
                TurnPages(1);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Z) && state == State.Opened)
                TurnPages(-1);
        }

        public void TurnPages(int pages)
        {
            if (pages == 0) return;

            Advancing = pages > 0;
            page = Mathf.Clamp(page + pages, 0, 11);
        }

        public async UniTask TakeOut()
        {
            state = State.Moving;

            await transform.LerpTransform(offShelfPosition, 0.3f);
            transform.LerpTransform(previewPosition, 1f).Forget();

            await UniTask.Delay(700);
            cameraTransform.DOMove(cameraPreviewLocation.position, 0.5f);
            cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.5f);
            await UniTask.Delay(250);
            if (fakeCover) fakeCover.SetActive(false);
            if (realCover) realCover.SetActive(true);
            if (realArmature) realArmature.SetActive(true);
            await UniTask.Delay(250);

            state = State.Previewing;
            ArchiveManager.Instance.currentBook = this;
        }

        public async UniTask PutBack()
        {
            if (!offShelfPosition) return;

            state = State.Moving;

            cameraTransform.DOMove(_cameraStartPos, 0.5f);
            cameraTransform.DORotate(_cameraStartRot, 0.5f);

            transform.DOMove(offShelfPosition.position, 1);
            transform.DORotate(offShelfPosition.eulerAngles, 1);
            await UniTask.Delay(250);
            if (fakeCover) fakeCover.SetActive(true);
            if (realCover) realCover.SetActive(false);
            if (realArmature) realArmature.SetActive(false);
            await UniTask.Delay(750);

            transform.DOMove(_startPos, 0.3f);
            transform.DORotate(_startRot, 0.3f);
            await UniTask.Delay(300);

            state = State.OnShelf;
            ArchiveManager.Instance.currentBook = null;
        }

        private async UniTask Open()
        {
            state = State.Moving;

            if (bookshelfObject) bookshelfObject.SetActive(false);
            Advancing = true;
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

            Advancing = false;
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
            if (memoryProgression && memoryProgression.state == MemoryProgression.State.Pending)
                await ArchiveManager.Instance.OnMemoryFinished(this);
        }

        public BookSpread GetCurrentSpread() => pageSpreads[page];
    }
}
