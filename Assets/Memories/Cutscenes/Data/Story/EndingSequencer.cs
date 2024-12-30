using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Memories.Cutscenes.Data.Story;

public class EndingSequencer : CustomSequencer
{
    public Transform cameraPosition;
    public GameObject deletePopup;

    public override async UniTask Play(CancellationToken ct = default)
    {
        Camera.main!.transform.DOMove(cameraPosition.position, 2).SetEase(Ease.InOutQuad);
        Camera.main!.transform.DORotate(cameraPosition.eulerAngles, 2).SetEase(Ease.InOutQuad);
        await UniTask.Delay(2000);

        deletePopup.SetActive(true);
    }

    public override void Skip()
    {
    }
}
