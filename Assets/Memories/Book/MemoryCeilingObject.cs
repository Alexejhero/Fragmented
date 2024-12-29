using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Memories.Book;

public class MemoryCeilingObject : MonoBehaviour
{
    private float _startLocalY;

    private void Awake()
    {
        _startLocalY = transform.localPosition.y;
    }

    public async UniTask DoDrop(float minDelay, float maxDelay, float minDropTime, float maxDropTime, float minYOffset, float maxYOffset)
    {
        gameObject.SetActive(true);

        transform.DOComplete();

        await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(minDelay, maxDelay)));

        float dropTime = Random.Range(minDropTime, maxDropTime);
        float targetY = transform.localPosition.y + Random.Range(minYOffset, maxYOffset);

        transform.DOLocalMoveY(targetY, dropTime).SetEase(Ease.OutBounce);
        await UniTask.Delay(TimeSpan.FromSeconds(dropTime));
    }

    public async UniTask DoRaise(float minDropTime, float maxDropTime)
    {
        transform.DOComplete();

        float raiseTime = Random.Range(minDropTime, maxDropTime) / 2;

        transform.DOLocalMoveY(_startLocalY, raiseTime).SetEase(Ease.InQuad);
        await UniTask.Delay(TimeSpan.FromSeconds(raiseTime));

        gameObject.SetActive(false);
    }
}
