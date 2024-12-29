using Cysharp.Threading.Tasks;
using Memories.Cutscenes;
using UnityEngine;

namespace Memories;

public sealed class MemoryProgression : MonoBehaviour
{
    public enum State
    {
        Locked,
        Pending,
        Viewed,
        Forgotten,
    }

    public Cutscene unlock;

    [Tooltip("Plays the first time you finish the book (followup neuro monologue etc)")]
    public Cutscene epilogue;

    public Cutscene forget;

    public State state = State.Locked;
    public bool IsAvailable => state is State.Pending or State.Viewed;

    public async UniTask Unlock()
    {
        Debug.Assert(state is State.Locked);

        // play vfx/animation
        if (unlock) await CutsceneManager.Instance.Play(unlock);

        state = State.Pending;
    }

    public async UniTask FirstClose()
    {
        Debug.Assert(state == State.Pending);
        state = State.Viewed;

        if (epilogue) await CutsceneManager.Instance.Play(epilogue);

        // todo: forget prompt confirmation thing
    }

    public async UniTask Forget()
    {
        Debug.Assert(state is State.Viewed);
        state = State.Forgotten;

        // play burn vfx/animation
        if (forget) await CutsceneManager.Instance.Play(forget);
    }
}
