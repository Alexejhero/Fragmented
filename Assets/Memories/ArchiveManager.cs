using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Helpers;
using UnityEngine;

namespace Memories;

public sealed class ArchiveManager : MonoSingleton<ArchiveManager>
{
    [Header("Data")]
    [Tooltip("Memories are unlocked in this order")]
    public List<Memory> allMemories;
    public Memory[] unlockAtStart;
    public int coreCapacity;

    [Header("Runtime")]
    public List<Memory> coreMemories = new();
    public IEnumerable<Memory> Available => allMemories.Where(m => m.IsAvailable);
    public IEnumerable<Memory> Forgotten => allMemories.Where(m => m.state is Memory.State.Forgotten);

    private void Start()
    {
        foreach (var m in unlockAtStart)
        {
            m.Unlock().Forget();
        }
    }

    public bool CanView(Memory memory)
    {
        return memory.IsAvailable && coreMemories.Count < coreCapacity;
    }
}
