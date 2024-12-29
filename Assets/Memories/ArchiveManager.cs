using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Helpers;
using Memories.Book;
using UnityEngine;

namespace Memories;

public sealed class ArchiveManager : MonoSingleton<ArchiveManager>
{
    [Header("Data")]
    [Tooltip("Memories are unlocked in this order")]
    public List<Memory> allMemories = new();
    public Memory[] unlockAtStart = Array.Empty<Memory>();
    public int coreCapacity = 3;

    [Header("Runtime")]
    public List<Memory> coreMemories = new();
    public IEnumerable<Memory> Available => allMemories.Where(m => m.IsAvailable);
    public IEnumerable<Memory> Forgotten => allMemories.Where(m => m.state is Memory.State.Forgotten);
    public MemoryBook currentBook;

    private void Start()
    {
        foreach (var m in unlockAtStart)
        {
            m.Unlock().Forget();
        }
    }

    public bool CanView(Memory memory)
    {
        Debug.Log($"CanView: {memory} {memory.state} {memory.IsAvailable}");
        return memory.IsAvailable && coreMemories.Count < coreCapacity;
    }
}
