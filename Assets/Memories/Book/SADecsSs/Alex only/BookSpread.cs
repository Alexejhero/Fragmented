using System;
using System.Linq;
using Memories.Book;
using Memories.Cutscenes;
using UnityEngine;

public class BookSpread : MonoBehaviour
{
    public int number;

    public MemoryCeiling ceiling;

    private MemoryBook _book;
    private BasePopup[] _popups;

    public Cutscene playOnOpen;
    public Cutscene playBeforeClose;
    public CustomSequencer[] customSequences;

    public Color lightColor = new Color32(255, 244, 214, 255);
    public float lightIntensity = 1;

    private Transform _leftPage;
    private Transform _rightPage;

    private float _lastValue;
    private bool _firstFrame = true;

    private void Awake()
    {
        _leftPage = transform.Find("L pivot");
        _rightPage = transform.Find("R pivot");

        _book = GetComponentInParent<MemoryBook>();
        _popups = GetComponentsInChildren<BasePopup>(true);
    }

    private void Update()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
            return;
        }

        float popupProgress = Math.Abs(_leftPage.localEulerAngles.y - _rightPage.localEulerAngles.y) % 360 / 180;

        if (!_book.Advancing) popupProgress *= -1;

        if (Mathf.Approximately(popupProgress, _lastValue)) return;
        _lastValue = popupProgress;

        foreach (BasePopup popup in _popups)
        {
            popup.DoRotate(popupProgress);
        }

        if (ceiling) ceiling.DoAction(popupProgress);
        if (Mathf.Approximately(Math.Abs(popupProgress), 1))
        {
            if (playOnOpen) playOnOpen.Play();
        }
    }

    public CustomSequencer GetSequencer(string sequenceName)
    {
        return customSequences.FirstOrDefault(s => s.sequenceName == sequenceName);
    }
}
