using System.Collections;
using UnityEngine;

namespace Memories.Sequencers;

public abstract class CutsceneSequencer : MonoBehaviour
{
    public abstract IEnumerator Play();
}
