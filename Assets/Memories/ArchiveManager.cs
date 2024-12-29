using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Helpers;
using Memories.Book;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Memories;

public sealed class ArchiveManager : MonoSingleton<ArchiveManager>
{
    [FormerlySerializedAs("allMemories")]
    public List<MemoryProgression> unlockOrder = new();

    private int _unlocked;
    public MemoryProgression[] unlockAtStart = Array.Empty<MemoryProgression>();

    [ShowInPlayMode]
    public MemoryBook currentBook;

    private void Start()
    {
        foreach (var m in unlockAtStart)
        {
            m.Unlock().Forget();
        }
        _unlocked = unlockAtStart.Length;
    }

    public async UniTask OnMemoryFinished(MemoryBook book)
    {
        // play any post-close cutscenes
        await book.memoryProgression.FirstClose();
        // burn current book
        // await RunForgetSequence(book);
        // unlock next book
        await unlockOrder[_unlocked++].Unlock();
    }
}
