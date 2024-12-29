using System;
using System.Collections;
using Helpers;
using TriInspector;
using UnityEngine;

namespace VFX.Book;

[AddComponentMenu("VFX/BookMaterialDriver")]
public class BookMaterialDriver : MonoBehaviour
{
    public Color dissolveColor = Color.white;
    public Color hoveringColor = Color.cyan;

    [Required]
    public Renderer bookRenderer;
    [Required]
    public Renderer dummyPagesRenderer;
    
    public float hoveringPowerPerUpdate = 3f;
    private float _hoveringPower = 0f;
    
#region IDForest
    private static readonly int DissolveFloatID = Shader.PropertyToID("_Dissolve");
    private static readonly int DissolveColorID = Shader.PropertyToID("_Dissolve_color");
    private static readonly int HoverColorID = Shader.PropertyToID("_Hover_color");
    private static readonly int DegreeOfLockedID = Shader.PropertyToID("_Degree_of_locked");
#endregion

    public void SetDefaults(bool isAvailable)
    {
        bookRenderer.material.SetFloat(DegreeOfLockedID, isAvailable ? 0f : 1f);
    }

    public void Unlock()
    {
        StartCoroutine(UnlockRoutine(1f));
    }
    
    public void Forget(float duration)
    {
        StartCoroutine(DissolveRoutine(duration));
    }

    private void OnMouseOver()
    {
        _hoveringPower += hoveringPowerPerUpdate * Time.deltaTime;
    }

    private void OnMouseExit()
    {
        _hoveringPower = 0f;
    }

    private void Update()
    {
        Color finalHoveringColor = new Color(hoveringColor.r, hoveringColor.g, hoveringColor.b, Math.Clamp(_hoveringPower, 0f, hoveringColor.a));
        bookRenderer.material.SetColor(HoverColorID, finalHoveringColor);
    }
    
    private IEnumerator UnlockRoutine(float duration)
    {
        yield return CommonCoroutines.DoOverTime(duration, t =>
        {
            bookRenderer.material.SetFloat(DegreeOfLockedID, 1 - t / duration);
        });
        bookRenderer.material.SetFloat(DegreeOfLockedID, 0f);
    }
    
    private IEnumerator DissolveRoutine(float duration)
    {
        bookRenderer.material.SetColor(DissolveColorID, dissolveColor);
        dummyPagesRenderer.material.SetColor(DissolveColorID, dissolveColor);
        yield return CommonCoroutines.DoOverTime(duration, t =>
        {
            bookRenderer.material.SetFloat(DissolveFloatID, t / duration);
            dummyPagesRenderer.material.SetFloat(DissolveFloatID, t / duration);

        });
        bookRenderer.material.SetFloat(DissolveFloatID, 1);
        dummyPagesRenderer.material.SetFloat(DissolveFloatID, 1);
    }
}
