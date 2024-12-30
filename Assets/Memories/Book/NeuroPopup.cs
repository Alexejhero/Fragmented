using System;
using Memories.Characters.Movement;
using UnityEngine;

namespace Memories.Book;

public class NeuroPopup : BasePopup
{
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
            Vector3 localPos = leftPage.parent.InverseTransformPoint(transform.position);

            if (Mathf.Abs(localPos.x) < 0.3f)
            {
                if (_lastLift < 0)
                {
                    transform.SetParent(leftPage, true);
                    transform.position = leftPage.parent.TransformPoint(new Vector3(Mathf.Lerp(localPos.x, 0.3f, Time.deltaTime * 2), localPos.y, localPos.z));
                }
                else if (_lastLift > 0)
                {
                    transform.SetParent(rightPage, true);
                    transform.position = leftPage.parent.TransformPoint(new Vector3(Mathf.Lerp(localPos.x, -0.3f, Time.deltaTime * 2), localPos.y, localPos.z));
                }
            }
        }

        Vector3 localPos2 = leftPage.parent.InverseTransformPoint(transform.position);
        if (_initialized) Debug.Log(localPos2);
    }

    public override void DoRotate(float lift)
    {
        Debug.Log(lift);
        _lastLift = lift;
    }
}
