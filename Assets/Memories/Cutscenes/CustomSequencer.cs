using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = FMOD.Debug;

namespace Memories.Cutscenes;

public abstract class CustomSequencer : MonoBehaviour
{
    public string sequenceName;
    public abstract UniTask Play(CancellationToken ct = default);
    public abstract void Skip();
}

