using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Helpers;
using Memories.Book;
using Memories.Cutscenes.Textbox;
using UnityEngine;

namespace Memories.Cutscenes;

public sealed class CutsceneManager : MonoSingleton<CutsceneManager>
{
    public Cutscene currentCutscene;

    private CancellationTokenSource _cts = new();

    private MemoryBook Book => FindObjectOfType<MainSceneScript>().activeBook;

    public async UniTask Play(Cutscene cutscene)
    {
        if (currentCutscene && currentCutscene != cutscene)
        {
            throw new InvalidOperationException("Cutscene already playing");
        }

        currentCutscene = cutscene;

        CancellationToken ct = _cts.Token;
        bool skipped = false;

        foreach (DialogueInstruction instr in cutscene.GetLines())
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

        Debug.Log($"Finished cutscene: {currentCutscene.data.cutsceneName}{(skipped ? " (skipped)" : "")}");

        cutscene.timesPlayed++;
        await TextboxManager.Instance.HideTopmost();
        currentCutscene = null;
    }

    public void SkipCutscene()
    {
        if (!currentCutscene) return;
        if (!currentCutscene.data.skippable) return;

        _cts.Cancel();
        _cts = new();
    }

    public async UniTask Execute(DialogueInstruction instruction, CancellationToken ct)
    {
        switch (instruction)
        {
            case TextLine textLine:
            {
                if (!textLine.actor) throw new InvalidOperationException($"Actor not assigned to \"{textLine.text}\"");

                Debug.Log($"[{textLine.actor.dialogueActorName}] {textLine.text}");
                await TextboxManager.Instance.Show(textLine.actor, textLine.text, ct);
                break;
            }
            case Pause pause:
            {
                Debug.Log($"pause {pause.duration}");
                await TextboxManager.Instance.HideTopmost();
                await UniTask.Delay(TimeSpan.FromSeconds(pause.duration), cancellationToken: ct);
                break;
            }
            case CustomSequence customSequence:
            {
                Debug.Log($"running {customSequence.sequenceName}");
                CustomSequencer sequence = Book.GetCurrentSpread().GetSequencer(customSequence.sequenceName);
                await sequence.Play(ct);
                break;
            }
            case MultipleWaitAll multi:
            {
                Debug.Log($"multi ({string.Join(',', multi.instructions.Select(i => i.GetType().Name))})");
                await UniTask.WhenAll(multi.instructions.Select(i => Execute(i, ct)));
                break;
            }
            case PlaySfx sfx:
            {
                Debug.Log($"sfx {sfx.sound.Path}");
                RuntimeManager.PlayOneShot(sfx.sound);
                break;
            }
            case TurnPages pages:
            {
                Debug.Log($"turning {pages.pages} pages");
                Book.TurnPages(pages.pages);
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
            case PlaySfx:
            {
                break;
            }
            case CustomSequence seq:
            {
                CustomSequencer sequence = Book.GetCurrentSpread().GetSequencer(seq.sequenceName);
                sequence.Skip();
                break;
            }
            case MultipleWaitAll multi:
            {
                foreach (DialogueInstruction inner in multi.instructions)
                    Skip(inner);
                break;
            }
            case TurnPages: // todo: erm... problem. how to instantly flip?
            {
                break;
            }
        }
    }
}
