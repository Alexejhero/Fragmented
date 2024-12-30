using System;
using System.Linq;
using System.Threading;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

    private MemoryBook Book
    {
        get
        {
            MainSceneScript main = FindObjectOfType<MainSceneScript>();
            return main != null ? main.activeBook : FindObjectOfType<MemoryBook>();
        }
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
        // non-skippable instructions
        if (currentCutscene.data.mainLines.Any(l => l is CloseBook)) return;

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
                await TextboxManager.Instance.Show(textLine.actor, textLine.text, dropdownAt: textLine is DropdownTextLine d ? d.dropdownAtChar : -1, ct: ct);
                break;
            }
            case ClearTextLine clearTextLine:
            {
                if (!clearTextLine.actor) throw new InvalidOperationException($"Actor not assigned to (clear)");
                Debug.Log($"[{clearTextLine.actor.dialogueActorName}] (clear)");
                TextboxManager.Instance.Clear(clearTextLine.actor);
                break;
            }
            case Pause pause:
            {
                Debug.Log($"pause {pause.duration}");
                if (!pause.keepTextbox) await TextboxManager.Instance.HideTopmost();
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
            case CloseBook:
            {
                Debug.Log("closing book");
                await Book.Close();
                break;
            }
            case SetMusicVolume vol:
            {
                Debug.Log($"vol {vol.volume}");
                DOTween.To(() => AudioSystem.SavedMusicVolume, AudioSystem.SetMusicVolume, vol.volume, vol.fadeDuration)
                    .SetEase(Ease.OutCubic);
                break;
            }
            case SetGlobalFmodParameter sgfp:
            {
                Debug.Log($"SetGlobalFmodParameter [{sgfp.soundEvent.Path}] {sgfp.parameterName}={sgfp.value}");
                RuntimeManager.StudioSystem.setParameterByName(sgfp.parameterName, sgfp.value);
                break;
            }
        }
    }

    public void Skip(DialogueInstruction instruction)
    {
        switch (instruction)
        {
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
            case CloseBook: // this one is just not skippable inherently
            {
                Book.Close().Forget();
                break;
            }
            case SetMusicVolume vol:
            {
                AudioSystem.SetMusicVolume(vol.volume);
                break;
            }
            case SetGlobalFmodParameter sgfp:
            {
                RuntimeManager.StudioSystem.setParameterByName(sgfp.parameterName, sgfp.value);
                break;
            }
            // remaining cases can be cleanly skipped
        }
    }
}
