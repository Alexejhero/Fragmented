using System.Collections;
using Helpers;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace VFX.Last_scene;

public class LastEffect : MonoBehaviour
{
    [FormerlySerializedAs("ps")] [Required]
    public ParticleSystem thanosParticles;
    public ParticleSystem fog;
    
    private float halfPSDuration;
    private static readonly int GlobalDissolveValueID = Shader.PropertyToID("_DissScene");
    [SerializeField]
    private float durationOfDissolveAsFractionOfTotalPSDuration = 0.5f;

    public float DurationSeconds => halfPSDuration;

    private void OnValidate()
    {
        Shader.SetGlobalFloat(GlobalDissolveValueID, 0);
    }

    private void Awake()
    {
        halfPSDuration = thanosParticles.main.duration + thanosParticles.main.startLifetime.Evaluate(1) * durationOfDissolveAsFractionOfTotalPSDuration;
        Shader.SetGlobalFloat(GlobalDissolveValueID, 0);
    }

    public void Play()
    {
        fog.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(DissolveCoroutine());
        thanosParticles.Play();
    }
    
    public IEnumerator WaitForFogGone() { yield return new WaitUntil(() => !fog.isPlaying);}
    
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
