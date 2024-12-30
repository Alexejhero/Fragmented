using UnityEngine;

public class RightPageGoesBelow : MonoBehaviour
{
    public MeshRenderer rend;

    private void Update()
    {
        rend.enabled = !Mathf.Approximately(transform.localEulerAngles.y, 0);
    }
}
