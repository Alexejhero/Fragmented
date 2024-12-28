using FMODUnity;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Memories.Cutscenes;

[CreateAssetMenu(menuName = "Cutscenes/Dialogue Actor", fileName = "DialogueActor", order = 0)]
public sealed class DialogueActor : ScriptableObject
{
    public Sprite face;
    public FontAsset font;
    public StudioEventEmitter talkNoise;
}
