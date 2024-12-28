using UnityEngine;

namespace Memories.Cutscenes;

[AddComponentMenu("Cutscenes/Waypoints")]
public sealed class CutsceneWaypoints : MonoBehaviour
{
    public Vector3[] waypoints;
    public int currentIndex;
}
