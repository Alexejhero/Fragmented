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

    private void Update()
    {
        if (Mathf.Approximately(_lastLift, -1) || Mathf.Approximately(_lastLift, 1))
        {
            popup.reverseRotation = (transform.localPosition.y > bookMidpoint) ^ invertCollapse;

            if (!groundTracker.surfaceCollider) return;

            if (groundTracker.surfaceCollider.transform.parent.name.Contains("left")) transform.SetParent(leftPage, true);
            else if (groundTracker.surfaceCollider.transform.parent.name.Contains("right")) transform.SetParent(rightPage, true);
        }
    }

    public override void DoRotate(float lift)
    {
        _lastLift = lift;
    }
}
