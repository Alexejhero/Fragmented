using System.Threading;
using Cysharp.Threading.Tasks;

namespace Memories.Cutscenes;

public sealed class ExampleSequence : CustomSequencer
{
    public int number;
    public override async UniTask Play(CancellationToken ct = default)
    {
        UnityEngine.Debug.Log($"Playing! {number}");
        await UniTask.Delay(2500, cancellationToken: ct);
    }

    public override void Skip()
    {
        UnityEngine.Debug.Log("Skipped!");
    }
}
