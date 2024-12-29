using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMODUnity;
using Helpers;
using Memories.Book;
using TriInspector;
using UnityEngine;

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

    public List<MemoryBook> unlockPile;
    public int maxUnlocked = 3;
    public int currentlyUnlocked = 0;

    private List<MemoryBook> _books;

    [GroupNext("Sounds")]
    public EventReference bookSlideIn;
    public EventReference bookSlideOut;
    public EventReference bookOpen;
    public EventReference bookClose;
    public EventReference bookPage;
    [UnGroupNext]

    private void Awake()
    {
        _cameraStartPos = cameraTransform.position;
        _cameraStartRot = cameraTransform.eulerAngles;

        _books = FindObjectsOfType<MemoryBook>().ToList();
    }

    private void Update()
    {
        while (currentlyUnlocked < maxUnlocked && unlockPile.Count > 0)
        {
            int index = Random.Range(0, unlockPile.Count);
            MemoryBook book = unlockPile[index];
            unlockPile.RemoveAt(index);

            book.Unlock();
            currentlyUnlocked++;
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
        _books.Where(b => b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(false));
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
        _books.Where(b => b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(true));
    }
}
