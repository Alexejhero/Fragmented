using System.Collections;
using Cysharp.Threading.Tasks;
using Memories.Sequencers;
using UnityEngine;

namespace Memories;

public class Memory : MonoBehaviour
{
    public enum State
    {
        Locked,
        New,
        Core,
        Forgotten,
    }

    [Tooltip("Plays on entering the memory (opening the book), before any gameplay.")]
    public CutsceneSequencer intro;

    [Tooltip("Plays on closing the book in the main archive (followup neuro monologue etc)")]
    public CutsceneSequencer outro;

    public State state = State.Locked;
    public bool IsAvailable => state is State.New or State.Core;

    public async UniTask Unlock()
    {
        Debug.Assert(state is State.Locked);

        // play vfx/animation

        await UniTask.Yield();
    }

    public IEnumerator Complete()
    {
        Debug.Assert(state is State.New);
        state = State.Core;

        // play close book vfx/animation

        yield return outro.Play();
    }

    public IEnumerator Forget()
    {
        Debug.Assert(state is State.Core);
        state = State.Forgotten;

        // play burn vfx/animation

        yield break;
    }
}
