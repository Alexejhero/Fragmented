using System.Collections;
using UnityEngine;

namespace Memories.Cutscenes;

public abstract class CustomSequencer : MonoBehaviour
{
    public abstract IEnumerator Play();
}
