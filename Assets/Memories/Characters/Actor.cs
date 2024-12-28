using Helpers;
using Memories.Book;
using UnityEngine;

namespace Memories.Characters;

public class Actor : MonoBehaviour
{
    public Popup popup;
    protected virtual void Awake()
    {
        this.EnsureComponent(ref popup);
    }
}
