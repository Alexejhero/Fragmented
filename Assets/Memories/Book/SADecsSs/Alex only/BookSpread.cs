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

        float popupProgress = Vector3.Angle(_leftPage.forward, _rightPage.forward) / 180;

        float leftAngle = Vector3.Angle(_leftPage.forward, Vector3.up);
        float rightAngle = 180 - Vector3.Angle(_rightPage.forward, Vector3.up);

        if (rightAngle > leftAngle) popupProgress *= -1;

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
