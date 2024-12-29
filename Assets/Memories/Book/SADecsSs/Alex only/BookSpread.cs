using Memories.Book;
using UnityEngine;

public class BookSpread : MonoBehaviour
{
    public int number;

    private MemoryBook _book;
    private Popup[] _popups;

    private void Awake()
    {
        _book = GetComponentInParent<MemoryBook>();
        _popups = GetComponentsInChildren<Popup>();
    }

    private void Update()
    {
        float popupProgress = 0;

        if (_book.pageSpreadProgress > number - 1 && _book.pageSpreadProgress < number)
        {
            popupProgress = _book.pageSpreadProgress - (number - 1);
        }
        else if (_book.pageSpreadProgress < number + 1 && _book.pageSpreadProgress > number)
        {
            popupProgress = number - _book.pageSpreadProgress + 1;
        }
        else if (Mathf.Approximately(_book.pageSpreadProgress, number))
        {
            popupProgress = 1;
        }

        foreach (Popup popup in _popups)
        {
           popup.DoRotate(popupProgress);
        }
    }
}
