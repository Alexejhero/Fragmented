using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMODUnity;
using Helpers;
using Memories.Book;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainSceneScript : MonoBehaviour
{
    public GameObject bookshelfObject;

    public Transform bookPreviewPosition;

    public Transform cameraTransform;
    public Transform cameraPreviewLocation;
    public Transform cameraReadingLocation;

    public bool busy = true;

    [HideInInspector]
    public MemoryBook activeBook;

    private Vector3 _cameraStartPos;
    private Vector3 _cameraStartRot;

    public List<MemoryBook> unlockOrderPrequeue;
    public List<MemoryBook> unlockOrderPostqueue;

    public List<MemoryBook> unlockPile;
    public int maxUnlocked = 3;
    public int currentlyUnlocked = 0;

    private int _prequeueIndex;

    private List<MemoryBook> _books;

    [GroupNext("Audio")]
    public EventReference bookSlideIn;
    public EventReference bookSlideOut;
    public EventReference bookOpen;
    public EventReference bookClose;
    public EventReference bookPage;
    public EventReference bookDelete;
    [UnGroupNext]

    public StudioEventEmitter endTrack;
    private bool _endgame;

    private void Awake()
    {
        _cameraStartPos = cameraTransform.position;
        _cameraStartRot = cameraTransform.eulerAngles;

        _books = FindObjectsOfType<MemoryBook>().ToList();
    }

    private void Update()
    {
        if (unlockOrderPrequeue.Count > 0) // we are in the prequeue
        {
            if (currentlyUnlocked == 0)
            {
                if (_prequeueIndex == unlockOrderPrequeue.Count)
                {
                    // escape the prequeue
                    unlockOrderPrequeue.Clear();
                }
                else
                {
                    MemoryBook book = unlockOrderPrequeue[_prequeueIndex];
                    book.Unlock();
                    _prequeueIndex++;
                    currentlyUnlocked++;
                }
            }
        }
        else if (unlockPile.Count > 0) // we are in the pile
        {
            if (currentlyUnlocked < maxUnlocked)
            {
                int index = Random.Range(0, unlockPile.Count);
                MemoryBook book = unlockPile[index];
                unlockPile.RemoveAt(index);

                book.Unlock();
                currentlyUnlocked++;
            }
        }
        else if (unlockOrderPostqueue.Count > 0) // we are in the postqueue
        {
            if (currentlyUnlocked == 0)
            {
                if (!_endgame)
                {
                    _endgame = true;
                    BackgroundMusic.Instance.SetTrack(endTrack);
                }

                MemoryBook book = unlockOrderPostqueue[0];
                unlockOrderPostqueue.RemoveAt(0);

                book.Unlock();
                currentlyUnlocked++;
            }
        }
    }

    public void TakeOutBook()
    {
        cameraTransform.DOMove(cameraPreviewLocation.position, 0.5f);
        cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.5f);
    }

    public void PutBackBook()
    {
        cameraTransform.DOMove(_cameraStartPos, 0.5f);
        cameraTransform.DORotate(_cameraStartRot, 0.5f);
    }

    public void OpenBook()
    {
        _books.Where(b => b && b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(false));
        bookshelfObject.SetActive(false);
        bookOpen.PlayOneShot();
        cameraTransform.DOMove(cameraReadingLocation.position, 0.85f);
        cameraTransform.DORotate(cameraReadingLocation.eulerAngles, 0.85f);
    }

    public async UniTask CloseBook()
    {
        cameraTransform.DOMove(cameraPreviewLocation.position, 0.7f);
        cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.7f);
        bookClose.PlayOneShot();
        await UniTask.Delay(700);
        bookshelfObject.SetActive(true);
        _books.Where(b => b && b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(true));
    }
}
