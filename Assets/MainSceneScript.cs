using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Memories.Book;
using UnityEngine;

public class MainSceneScript : MonoBehaviour
{
    public GameObject bookshelfObject;

    public Transform bookPreviewPosition;

    public Transform cameraTransform;
    public Transform cameraPreviewLocation;
    public Transform cameraReadingLocation;

    public new Light light;

    [HideInInspector]
    public MemoryBook activeBook;

    private Vector3 _cameraStartPos;
    private Vector3 _cameraStartRot;

    private List<MemoryBook> _books;

    private void Awake()
    {
        _cameraStartPos = cameraTransform.position;
        _cameraStartRot = cameraTransform.eulerAngles;

        _books = FindObjectsOfType<MemoryBook>().ToList();
    }

    public void TakeOutBook()
    {
        cameraTransform.DOMove(cameraPreviewLocation.position, 0.5f);
        cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.5f);
        if (light) light.DOIntensity(0, 0.5f);
    }

    public void PutBackBook()
    {
        cameraTransform.DOMove(_cameraStartPos, 0.5f);
        cameraTransform.DORotate(_cameraStartRot, 0.5f);
        if (light) light.DOIntensity(1, 0.5f);
    }

    public void OpenBook()
    {
        _books.Where(b => b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(false));
        bookshelfObject.SetActive(false);
        cameraTransform.DOMove(cameraReadingLocation.position, 0.85f);
        cameraTransform.DORotate(cameraReadingLocation.eulerAngles, 0.85f);
    }

    public async UniTask CloseBook()
    {
        cameraTransform.DOMove(cameraPreviewLocation.position, 0.7f);
        cameraTransform.DORotate(cameraPreviewLocation.eulerAngles, 0.7f);
        await UniTask.Delay(700);
        bookshelfObject.SetActive(true);
        _books.Where(b => b != activeBook).ToList().ForEach(b => b.gameObject.SetActive(true));
    }
}
