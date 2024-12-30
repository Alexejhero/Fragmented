using System.Threading;
using Cysharp.Threading.Tasks;
using Memories.Cutscenes;
using UnityEngine;

namespace Memories.Mechanisms;

public sealed class EnableTargets : CustomSequencer
{
    public GameObject[] objects;
    public Behaviour[] components;
    public override UniTask Play(CancellationToken ct = default)
    {
        DoEnable();
        return UniTask.CompletedTask;
    }

    public override void Skip()
    {
        DoEnable();
    }

    private void DoEnable()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }

        foreach (Behaviour comp in components)
        {
            comp.enabled = true;
        }
    }
}
