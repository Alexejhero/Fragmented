using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Memories.Cutscenes;

[CreateAssetMenu(menuName = "Cutscenes/Cutscene", fileName = "Cutscene", order = 0)]
public sealed class Cutscene : ScriptableObject
{
    [SerializeReference]
    public DialogueInstruction[] mainLines;
    [SerializeReference]
    public List<DialogueInstruction[]> repeatLines;
}


[Serializable]
public abstract class DialogueInstruction
{
}

public sealed class Pause : DialogueInstruction
{
    public float duration;
}

public sealed class TextLine : DialogueInstruction
{
    public DialogueActor actor;
    [TextArea]
    public string text;
}

public sealed class SendMessage : DialogueInstruction
{
    [InfoBox("Make sure the object name is unique in the scene.")]
    public string targetObjectName;
    public string message;
}
