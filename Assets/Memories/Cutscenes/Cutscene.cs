using System;
using System.Collections.Generic;
using Memories.Characters;
using TriInspector;
using UnityEngine;

namespace Memories.Cutscenes;

[AddComponentMenu("Cutscenes/Cutscene")]
public sealed class Cutscene : MonoBehaviour
{
    public enum RepeatBehaviour
    {
        Last,
        Random,
        Loop,
    }

    public string cutsceneName;
    public int timesPlayed;
    public RepeatBehaviour repeatBehaviour;

    [SerializeReference]
    public DialogueInstruction[] mainLines;
    [SerializeReference]
    public List<DialogueInstruction[]> repeatLines;
}


[Serializable]
public abstract class DialogueInstruction
{
}

[Serializable]
public sealed class Pause : DialogueInstruction
{
    public float duration;
}

[Serializable]
public sealed class TextLine : DialogueInstruction
{
    public BookActor actor;
    [TextArea]
    public string text;
}

[Serializable]
public sealed class CustomSequence : DialogueInstruction
{
    public CustomSequencer sequencer;
}
