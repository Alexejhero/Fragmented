namespace Memories.Book;

public class ObjectEnablerPopup : BasePopup
{
    public float leftThreshold = -0.1f;
    public float rightThreshold = 0.1f;

    public override void DoRotate(float lift)
    {
        gameObject.SetActive(lift < leftThreshold || lift > rightThreshold);
    }
}
