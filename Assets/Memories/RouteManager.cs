using System.Collections.Generic;
using Helpers;
using Memories.Data;
using UnityEngine;

namespace Memories;

public sealed class RouteManager : MonoSingleton<RouteManager>
{
    // data
    public List<Memory> allMemories;

    // runtime
    public int mood;
    public List<Memory> completedMemories = new();
    public List<Memory> forgottenMemories = new();

    public void Complete(Memory memory)
    {
        completedMemories.Add(memory);
    }

    public void Forget(Memory memory)
    {
        Debug.Assert(completedMemories.Contains(memory));
        forgottenMemories.Add(memory);
    }
}
