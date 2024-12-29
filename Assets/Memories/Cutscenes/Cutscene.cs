using System;
using Cysharp.Threading.Tasks;
using Memories.Book;
using TriInspector;
using UnityEngine;

namespace Memories.Cutscenes;

[AddComponentMenu("Cutscenes/Cutscene")]
public sealed class Cutscene : MonoBehaviour
{
    [HideInEditMode]
    public int timesPlayed;
    public CutsceneData data;

    private int _repeatLoopIndex;

    public DialogueInstruction[] GetLines()
    {
        if (timesPlayed == 0 || !data.repeatable)
            return data.mainLines;

        int i = _repeatLoopIndex++;
        i = data.repeatBehaviour switch
        {
            CutsceneData.RepeatBehaviour.Last => Math.Max(i, data.repeatLines.Length - 1),
            CutsceneData.RepeatBehaviour.Loop => i % data.repeatLines.Length,
            CutsceneData.RepeatBehaviour.Random => UnityEngine.Random.Range(0, data.repeatLines.Length),
            _ => i,
        };

        return data.repeatLines[i].lines;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            Play();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
        {
            Skip();
        }
    }

    [Button, ShowInPlayMode]
    public void Play()
    {
        if (CutsceneManager.Instance.currentCutscene) return;

        // todo: temp for testing
        ArchiveManager.Instance.currentBook = GameObject.FindObjectOfType<MemoryBook>();

        CutsceneManager.Instance.Play(this).Forget();
    }

    [Button, ShowInPlayMode]
    public void Skip()
    {
        if (CutsceneManager.Instance.currentCutscene != this) return;

        CutsceneManager.Instance.SkipCutscene();
    }
}
