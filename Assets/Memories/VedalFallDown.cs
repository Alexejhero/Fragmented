using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Memories.Cutscenes;
using UnityEngine;
using UnityEngine.Serialization;

public class VedalFallDown : CustomSequencer
{
    public Vector3 offPlankPosition;
    [FormerlySerializedAs("targetPosition")] public Vector3 targetLocalPosition;

    public override async UniTask Play(CancellationToken ct = default)
    {
        transform.DOLocalMove(offPlankPosition, 0.2f).SetEase(Ease.InQuad);
        await UniTask.Delay(200);
        transform.DOLocalMove(targetLocalPosition, 1).SetEase(Ease.InBounce);
    }

    public override void Skip()
    {
    }
}
