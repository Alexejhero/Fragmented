using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Memories.Cutscenes;

public sealed class CutsceneManager : MonoBehaviour
{
    public Cutscene currentCutscene;

    private CancellationTokenSource _cts;
    public async UniTask Play(Cutscene cutscene)
    {
        if (currentCutscene && currentCutscene != cutscene)
        {
            throw new InvalidOperationException("Cutscene already playing");
        }

        currentCutscene = cutscene;

        CancellationToken ct = _cts.Token;
        bool skipped = false;

        foreach (var instr in cutscene.mainLines)
        {
            if (skipped)
            {
                Skip(instr);
            }
            else if (await Execute(instr, ct).SuppressCancellationThrow())
            {
                skipped = true;
            }
        }
        currentCutscene = null;
    }

    public void SkipCutscene()
    {
        _cts.Cancel();
        _cts = new();
    }

    public async UniTask Execute(DialogueInstruction instruction, CancellationToken ct)
    {
        switch (instruction)
        {
            case TextLine textLine:
            {
                await TextboxManager.Instance.Show(textLine.actor, textLine.text, ct);
                break;
            }
            case Pause pause:
            {
                await UniTask.Delay(TimeSpan.FromSeconds(pause.duration), cancellationToken: ct);
                break;
            }
            case CustomSequence customSequence:
            {
                await customSequence.sequencer.Play(ct);
                break;
            }
        }
    }

    public void Skip(DialogueInstruction instruction)
    {
        switch (instruction)
        {
            case TextLine:
            case Pause:
                break;
            case CustomSequence seq:
                seq.sequencer.Skip();
                break;
        }
    }
}
