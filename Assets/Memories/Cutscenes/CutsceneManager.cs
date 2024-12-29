using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Helpers;
using Memories.Book;
using Memories.Characters;

namespace Memories.Cutscenes;

public sealed class CutsceneManager : MonoSingleton<CutsceneManager>
{
    public Cutscene currentCutscene;
    private readonly List<BookActor> _actors = new();

    private CancellationTokenSource _cts;

    private MemoryBook Book => ArchiveManager.Instance.currentBook;

    public void Load(MemoryBook book)
    {
        book.GetComponentsInChildren(true, _actors);
    }

    public async UniTask Play(Cutscene cutscene)
    {
        if (currentCutscene && currentCutscene != cutscene)
        {
            throw new InvalidOperationException("Cutscene already playing");
        }

        currentCutscene = cutscene;

        CancellationToken ct = _cts.Token;
        bool skipped = false;

        foreach (DialogueInstruction instr in cutscene.data.mainLines)
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
                BookActor actor = _actors.Find(a => a.dialogueActorName == textLine.dialogueActorName);
                await TextboxManager.Instance.Show(actor, textLine.text, ct);
                break;
            }
            case Pause pause:
            {
                await UniTask.Delay(TimeSpan.FromSeconds(pause.duration), cancellationToken: ct);
                break;
            }
            case CustomSequence customSequence:
            {
                CustomSequencer sequence = Book.GetSequencer(customSequence.sequenceName);
                await sequence.Play(ct);
                break;
            }
            case MultipleWaitAll multi:
            {
                await UniTask.WhenAll(multi.instructions.Select(i => Execute(i, ct)));
                break;
            }
            case PlaySfx sfx:
            {
                RuntimeManager.PlayOneShot(sfx.sound);
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
            {
                break;
            }
            case CustomSequence seq:
            {
                CustomSequencer sequence = Book.GetSequencer(seq.sequenceName);
                sequence.Skip();
                break;
            }
            case MultipleWaitAll multi:
            {
                foreach (DialogueInstruction inner in multi.instructions)
                    Skip(inner);
                break;
            }
            case PlaySfx sfx:
            {
                // RuntimeManager.PlayOneShot(sfx.sound);
                break;
            }
        }
    }
}
