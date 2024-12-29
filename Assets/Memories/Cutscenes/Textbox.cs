using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Memories.Cutscenes;

public sealed class Textbox : MonoBehaviour
{
    public TMP_Text tmp;
    [Tooltip("Characters revealed per second")]
    public int speed = 10;

    // todo adjust
    public float commaPause = 0.5f;
    public float sentencePause = 0.7f;
    public float ellipsisPause = 1.5f;

    private void Awake()
    {
        TextboxManager.Instance.Register(this);
    }

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
        await TickCharacters(text, linkedCts.Token);
        linkedCts.Cancel();
    }

    private async UniTask TickCharacters(string text, CancellationToken ct = default)
    {
        tmp.text = text;
        for (int i = 0; i < text.Length; i++)
        {
            if (ct.IsCancellationRequested)
            {
                tmp.maxVisibleCharacters = text.Length;
                return;
            }
            tmp.maxVisibleCharacters = i;
            float delay = text[i] switch
            {
                ',' => commaPause,
                '.' when i>1 && text[i-1] == '.' && text[i-2] == '.'
                    => ellipsisPause,
                '.' or '!' or '?' when i+1 < text.Length && !char.IsPunctuation(text[i+1])
                    => sentencePause,
                _ => 1f / speed,
            };
            await UniTask.Delay(Mathf.RoundToInt(1000 * delay), cancellationToken: ct);
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
