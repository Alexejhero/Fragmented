using UnityEngine;

namespace Memories.Book;

public abstract class BasePopup : MonoBehaviour
{
    protected void Start()
    {
        DoRotate(0);
    }

    public abstract void DoRotate(float lift);
}
