using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memories.Cutscenes;
using UnityEngine;

public class Story10Scuff : MonoBehaviour
{
    public List<MonoBehaviour> components;
    public Cutscene cutscene;

    private void Update()
    {
        if (components.All(c => !c))
        {
            if (CutsceneManager.Instance.currentCutscene) return;

            cutscene.Play();

            Destroy(this);
        }
    }
}
