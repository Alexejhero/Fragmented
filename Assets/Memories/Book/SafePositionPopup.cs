using UnityEngine;

namespace Memories.Book;

public class SafePositionPopup : BasePopup
{
    public float leftThreshold = -0.95f;
    public float rightThreshold = 0.95f;

    private Vector3 _startLocalPosition;

    private void Awake()
    {
        _startLocalPosition = transform.localPosition;
    }

    public override void DoRotate(float lift)
    {
        if (lift < leftThreshold || lift > rightThreshold) return;

        lift = Mathf.Abs(lift);

        transform.localPosition = Vector3.Lerp(transform.localPosition, _startLocalPosition, lift);
    }
}
