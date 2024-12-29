using UnityEngine;

namespace Memories.Book;

public class ComponentEnablerPopup : BasePopup
{
    public float leftThreshold = -0.1f;
    public float rightThreshold = 0.1f;

    public MonoBehaviour[] components;

    public override void DoRotate(float lift)
    {
        foreach (MonoBehaviour component in components)
        {
            component.enabled = lift < leftThreshold || lift > rightThreshold;
        }
    }
}
