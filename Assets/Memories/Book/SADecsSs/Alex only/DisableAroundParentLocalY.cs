using UnityEngine;

public class DisableAroundParentLocalY : MonoBehaviour
{
    public new Collider collider;
    public float localEulerY;

    private void Update()
    {
        if (collider) collider.enabled = !Mathf.Approximately(transform.parent.localEulerAngles.y, localEulerY);
    }
}
