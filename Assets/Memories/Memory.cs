using System.Collections;
using Cysharp.Threading.Tasks;
using Memories.Book;
using Memories.Cutscenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Memories;

public class Memory : MonoBehaviour, IPointerClickHandler
{
    public enum State
    {
        Locked,
        New,
        Core,
        Forgotten,
    }

    public MemoryBook book;

    [Tooltip("Plays on entering the memory (opening the book), before any gameplay.")]
    public CustomSequencer intro;

    [Tooltip("Plays on closing the book in the main archive (followup neuro monologue etc)")]
    public CustomSequencer outro;

    public State state = State.Locked;
    public bool IsAvailable => state is State.New or State.Core;

    public async UniTask Unlock()
    {
        Debug.Assert(state is State.Locked);

        // play vfx/animation

        await UniTask.Yield();
    }

    public IEnumerator Open()
    {
        Debug.Assert(ArchiveManager.Instance.CanView(this));

        // play open book vfx/animation
        yield return book.TakeOut().ToCoroutine();

        yield return intro.Play();
    }

    public IEnumerator Close()
    {
        Debug.Assert(ArchiveManager.Instance.CanView(this));
        state = State.Core;

        // play close book vfx/animation

        yield return outro.Play();

        ArchiveManager.Instance.coreMemories.Add(this);
    }

    public IEnumerator Forget()
    {
        Debug.Assert(state is State.Core);
        state = State.Forgotten;

        // play burn vfx/animation

        yield break;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!ArchiveManager.Instance.CanView(this)) return;

        StartCoroutine(Open());
    }
}
