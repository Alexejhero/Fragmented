using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class Fader
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
}
