using System.Threading;
using Cysharp.Threading.Tasks;
using Memories.Characters;
using TMPro;
using UnityEngine;

namespace Memories.Cutscenes;

public sealed class Textbox : MonoBehaviour
{
    public BookActor actor;
    public TMP_Text tmp;

    private void Awake()
    {
        if (!actor) actor = GetComponentInParent<BookActor>();
        TextboxManager.Instance.Register(this);
    }

    public async UniTask Show(string text, CancellationToken ct = default)
    {
        await DisplayText(text, ct);
        await WaitForAdvance();
    }

    public async UniTask DisplayText(string text, CancellationToken ct = default)
    {
        // do whatever TMP text thing
    }

    private readonly AutoResetUniTaskCompletionSource _advanceSource = AutoResetUniTaskCompletionSource.Create();
    public UniTask WaitForAdvance() => _advanceSource.Task;

    private void Advance()
    {
        _advanceSource.TrySetResult();
    }

    // input system, triggers when clicking anywhere
    public void OnClick()
    {
        Advance();
    }
}
