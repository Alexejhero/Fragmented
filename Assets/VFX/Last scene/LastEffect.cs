using System.Collections;
using Helpers;
using TriInspector;
using UnityEngine;

namespace VFX.Last_scene;

public class LastEffect : MonoBehaviour
{
    [Required]
    public ParticleSystem ps;
    private float halfPSDuration;
    private static readonly int GlobalDissolveValueID = Shader.PropertyToID("_DissScene");
    public float durationOfDissolveAsFractionOfTotalPSDuration = 0.5f;

    private void OnValidate()
    {
        Shader.SetGlobalFloat(GlobalDissolveValueID, 0);
    }

    private void Awake()
    {
        halfPSDuration = ps.main.duration + ps.main.startLifetime.Evaluate(1) * durationOfDissolveAsFractionOfTotalPSDuration;
        Shader.SetGlobalFloat(GlobalDissolveValueID, 0);
    }

    public void Play()
    {
        StartCoroutine(DissolveCoroutine());
        ps.Play();
    }
    
    private IEnumerator DissolveCoroutine()
    {
        Shader.SetGlobalFloat(GlobalDissolveValueID, 0);
        yield return CommonCoroutines.DoOverTime(halfPSDuration, t =>
        {
            Shader.SetGlobalFloat(GlobalDissolveValueID, t / halfPSDuration);
        });
        Shader.SetGlobalFloat(GlobalDissolveValueID, 1);
    }
}
