using Cysharp.Threading.Tasks;
using Memories.Book;
using Memories.Cutscenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Memories;

public sealed class Memory : MonoBehaviour, IPointerClickHandler
{
    public enum State
    {
        Locked,
        New,
        Core,
        Forgotten,
    }

    public MemoryBook book;

    public Cutscene unlock;

    [Tooltip("Plays on entering the memory (opening the book), before any gameplay.")]
    public Cutscene intro;

    [Tooltip("Plays on closing the book in the main archive (followup neuro monologue etc)")]
    public Cutscene outro;

    public Cutscene forget;

    public State state = State.Locked;
    public bool IsAvailable => state is State.New or State.Core;

    public async UniTask Unlock()
    {
        Debug.Assert(state is State.Locked);

        // play vfx/animation
        await CutsceneManager.Instance.Play(unlock);
    }

    public async UniTask Open()
    {
        Debug.Assert(ArchiveManager.Instance.CanView(this));

        // play open book vfx/animation
        await book.TakeOut();

        CutsceneManager.Instance.Load(book);

        await CutsceneManager.Instance.Play(intro);
    }

    public async UniTask Close()
    {
        Debug.Assert(ArchiveManager.Instance.CanView(this));
        state = State.Core;

        // play close book vfx/animation
        // await book.PutBack();

        await CutsceneManager.Instance.Play(outro);

        ArchiveManager.Instance.coreMemories.Add(this);
    }

    public async UniTask Forget()
    {
        Debug.Assert(state is State.Core);
        state = State.Forgotten;

        // play burn vfx/animation
        await CutsceneManager.Instance.Play(forget);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!ArchiveManager.Instance.CanView(this)) return;

        Open().Forget();
    }
}
