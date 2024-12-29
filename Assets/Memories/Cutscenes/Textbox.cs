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
            await UniTask.Delay(1000 / speed, cancellationToken: ct);
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
