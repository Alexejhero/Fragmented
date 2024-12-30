using UnityEngine;

namespace Memories.Book;

public class CaveStreamVII : BasePopup
{
    public Vector3 zeroLocalPos;
    public Vector3 oneLocalPos;

    public override void DoRotate(float lift)
    {
        lift = Mathf.Abs(lift);
        transform.localPosition = Vector3.Lerp(zeroLocalPos, oneLocalPos, lift);
    }
}
