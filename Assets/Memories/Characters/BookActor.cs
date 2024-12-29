using Helpers;
using Memories.Book;
using Memories.Cutscenes;
using UnityEngine;

namespace Memories.Characters;

public class BookActor : MonoBehaviour
{
    public Popup popup;

    public Textbox textbox;
    // (maybe) when a character speaks, the camera zooms in on them
    public Transform dialogueSpeakerCameraTarget;
    protected virtual void Awake()
    {
        this.EnsureComponent(ref popup);
    }
}
