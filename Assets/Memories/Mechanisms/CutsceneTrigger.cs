using Memories.Characters.Movement;
using Memories.Cutscenes;

namespace Memories.Mechanisms;

public sealed class CutsceneTrigger : Trigger<PlayerController>
{
    public Cutscene cutscene;
    protected override void OnEnter(PlayerController target)
    {
        if (CutsceneManager.Instance.currentCutscene) return;

        cutscene.Play();
        if (!cutscene.data.repeatable)
            Destroy(this);
    }
}
