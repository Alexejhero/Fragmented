using Memories.Book;
using UnityEngine;

public class BookSpread : MonoBehaviour
{
    public int number;

    private MemoryBook _book;
    private BasePopup[] _popups;

    private Transform _leftPage;
    private Transform _rightPage;

    private float _lastValue;

    private void Awake()
    {
        _leftPage = transform.Find("L pivot");
        _rightPage = transform.Find("R pivot");

        _book = GetComponentInParent<MemoryBook>();
        _popups = GetComponentsInChildren<BasePopup>(true);
    }

    private void Update()
    {
        float popupProgress = (_leftPage.localEulerAngles.y - _rightPage.localEulerAngles.y + 360) % 360 / 180;
        if (Mathf.Abs(popupProgress) < 0.01f) popupProgress = 0;
        if (popupProgress > 1.7f) popupProgress = 0;
        if (popupProgress > 1) popupProgress = 1;

        if (!_book.Advancing) popupProgress *= -1;

        if (Mathf.Approximately(popupProgress, _lastValue)) return;
        _lastValue = popupProgress;

        foreach (BasePopup popup in _popups)
        {
            popup.DoRotate(popupProgress);
        }
    }
}
