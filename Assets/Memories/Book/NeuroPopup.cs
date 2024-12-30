using System;
using Memories.Characters.Movement;
using UnityEngine;

namespace Memories.Book;

public class NeuroPopup : BasePopup
{
    public Collider neuroCollider;
    public GroundTracker groundTracker;
    public Popup popup;

    public Transform leftPage;
    public Transform rightPage;

    public float bookMidpoint = 0.6f;
    public bool invertCollapse;

    private float _lastLift = 0;
    private bool _initialized;

    private void Awake()
    {
        if (!leftPage || !rightPage) throw new Exception("Left and right pages must be assigned to the spread where neuro is on!");
    }

    private void Update()
    {
        if (Mathf.Approximately(Mathf.Abs(_lastLift), 1))
        {
            _initialized = true;

            popup.reverseRotation = (transform.localPosition.y > bookMidpoint) ^ invertCollapse;

            if (!groundTracker.surfaceCollider) return;

            if (groundTracker.surfaceCollider.transform.parent.name.Contains("left")) transform.SetParent(leftPage, true);
            else if (groundTracker.surfaceCollider.transform.parent.name.Contains("right")) transform.SetParent(rightPage, true);
        }
        else if (_initialized)
        {
            if (Mathf.Abs(transform.localPosition.x) < 0.3f)
            {
                transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, 0.3f, Time.deltaTime * 2), transform.localPosition.y, transform.localPosition.z);
                neuroCollider.enabled = false;
            }
        }

        Debug.Log(transform.localPosition.x);
    }

    public override void DoRotate(float lift)
    {
        _lastLift = lift;
    }
}
