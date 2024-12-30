using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class Lerper
{
    public static async UniTask FadeAlpha(this Graphic graphic, float alpha, float duration)
    {
        Color startColor = graphic.color;
        Color endColor = new(startColor.r, startColor.g, startColor.b, alpha);
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            graphic.color = Color.Lerp(startColor, endColor, t);
            await UniTask.Yield();
        }
        graphic.color = endColor;
    }

    public static async UniTask LerpTransform(this Transform transform, Transform target, float duration)
    {
        transform.DOMove(target.position, duration)
            .SetEase(Ease.OutCubic);
        transform.DORotate(target.eulerAngles, duration)
            .SetEase(Ease.OutCubic);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        return;
        Vector3 targetPosition = target.position;
        Quaternion targetRotation = target.rotation;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            await UniTask.Yield();
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
