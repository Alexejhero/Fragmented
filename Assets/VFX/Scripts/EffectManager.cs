using System.Collections;
using Helpers;
using UnityEngine;

namespace VFX.Scripts;

public sealed class EffectManager : MonoSingleton<EffectManager>
{
    
#region IDForest
    private static readonly int FullScreenColorFromID = Shader.PropertyToID("_FullScreenColorFrom");
    private static readonly int FullScreenColorToID = Shader.PropertyToID("_FullScreenColorTo");
    private static readonly int FullScreenColorPhaseID = Shader.PropertyToID("_FullScreenColorPhase");
#endregion

    public void FullScreenFadeIn(Color colorFrom, float duration, AnimationCurve curve  = null)
    {
        StartCoroutine(Fade(colorFrom, new Color(colorFrom.r, colorFrom.g, colorFrom.b, 0), duration, curve));
    }
    
    public void FullScreenFadeBlend(Color colorFrom, Color colorTo, float duration, AnimationCurve curve  = null)
    {
        StartCoroutine(Fade(colorFrom, colorTo , duration, curve));
    }

    public void FullScreenFadeOut(Color colorTo, float duration, AnimationCurve curve  = null)
    {
        StartCoroutine(Fade(new Color(colorTo.r, colorTo.g, colorTo.b, 0), colorTo, duration, curve));
    }
    
    private static IEnumerator Fade(Color from, Color to, float duration, AnimationCurve curve  = null)
    {
        Shader.SetGlobalFloat(FullScreenColorPhaseID, 0);
        yield return CommonCoroutines.DoOverTime(duration, t =>
        {
            Shader.SetGlobalColor(FullScreenColorFromID, from);
            Shader.SetGlobalColor(FullScreenColorToID, to);
            Shader.SetGlobalFloat(FullScreenColorPhaseID, curve?.Evaluate(t / duration) ?? t / duration);
        });
        Shader.SetGlobalColor(FullScreenColorToID, to);
    }
}
