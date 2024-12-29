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
        await DisplayText(text, actor, ct);
        await WaitForAdvance(ct);
    }

    public async UniTask DisplayText(string text, DialogueActorData actor, CancellationToken ct = default)
    {
        tmp.text = "";
        CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        // skip and display all
        WaitForAdvance(linkedCts.Token).ContinueWith(() => linkedCts.Cancel()).Forget();
        await TickCharacters(text, actor, linkedCts.Token);
        linkedCts.Cancel();
    }

    private async UniTask TickCharacters(string text, DialogueActorData actor, CancellationToken ct = default)
    {
        tmp.color = actor.textColor;
        tmp.text = text;
        for (int i = 0; i < text.Length; i++)
        {
            if (ct.IsCancellationRequested)
            {
                tmp.maxVisibleCharacters = text.Length;
                return;
            }
            tmp.maxVisibleCharacters = i;
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

    private readonly AutoResetUniTaskCompletionSource _advanceSource = AutoResetUniTaskCompletionSource.Create();
    public UniTask WaitForAdvance(CancellationToken ct = default) => _advanceSource.Task.AttachExternalCancellation(ct);

    private void Advance()
    {
        _advanceSource.TrySetResult();
    }

    // input system, triggers when clicking anywhere (or pressing the key bound to Submit)
    public void OnClick()
    {
        Advance();
    }
}
