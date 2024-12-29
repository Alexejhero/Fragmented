using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Memories.Cutscenes;

public abstract class CustomSequencer : MonoBehaviour
{
    public abstract UniTask Play(CancellationToken ct = default);
    public abstract void Skip();
}
