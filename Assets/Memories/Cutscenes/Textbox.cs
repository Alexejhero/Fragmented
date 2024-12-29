using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Memories.Cutscenes;

public sealed class Textbox : MonoBehaviour
{
    public TMP_Text tmp;
    [Tooltip("Characters revealed per second")]
    public int speed = 10;

    // delay equivalent to x characters
    // todo: adjust
    [FormerlySerializedAs("commaPause")]
    public int commaPauseCharacters = 5;
    [FormerlySerializedAs("sentencePause")]
    public int sentencePauseCharacters = 7;
    [FormerlySerializedAs("ellipsisPause")]
    public int ellipsisPauseCharacters = 15;

    public async UniTask Show(string text, DialogueActorData actor, CancellationToken ct = default)
    {
        // await ShowPrefab(); // raise popup
        await DisplayText(text, actor, ct);
        await WaitForAdvance(ct);
        // await HidePrefab(); // lower popup
    }

    public async UniTask DisplayText(string text, DialogueActorData actor, CancellationToken ct = default)
    {
        tmp.text = "";
        CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        try
        {
            // ReSharper disable once AccessToDisposedClosure
            WaitForAdvance(linkedCts.Token) // skip and display all
                .ContinueWith(linkedCts.Cancel).Forget();
            bool skipped = await TickCharacters(text, actor, linkedCts.Token).SuppressCancellationThrow();
            if (skipped)
            {
                tmp.maxVisibleCharacters = text.Length;
            }
        }
        finally
        {
            linkedCts.Cancel();
            linkedCts.Dispose();
        }
    }

    private async UniTask TickCharacters(string text, DialogueActorData actor, CancellationToken ct = default)
    {
        tmp.color = actor.textColor;
        tmp.text = text;
        for (int i = 0; i < text.Length; i++)
        {
            tmp.maxVisibleCharacters = i + 1;
            float delayPerCharacter = 1f / speed;
            float delayCharacters = delayPerCharacter * text[i] switch
            {
                ',' => commaPauseCharacters,
                '.' when i>1 && text[i-1] == '.' && text[i-2] == '.'
                    => ellipsisPauseCharacters,
                '.' or '!' or '?' when i+1 < text.Length && !char.IsPunctuation(text[i+1])
                    => sentencePauseCharacters,
                _ => 1,
            };
            if (!actor.talkNoise.IsNull)
            {
                RuntimeManager.PlayOneShot(actor.talkNoise);
            }
            await UniTask.Delay(Mathf.RoundToInt(1000 * delayCharacters), cancellationToken: ct);
        }
    }

    private UniTaskCompletionSource _advanceSource = new();
    public UniTask WaitForAdvance(CancellationToken ct = default) => _advanceSource.Task.AttachExternalCancellation(ct);

    private void Advance()
    {
        Debug.Log("Advance");
        _advanceSource.TrySetResult();
        _advanceSource = new();
    }

    private void Update()
    {
        // todo: temp
        if (UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
            Advance();
    }

    // input system, triggers when clicking anywhere (or pressing the key bound to Submit)
    public void OnClick()
    {
        Debug.Log("Clicked");
        Advance();
    }
}
