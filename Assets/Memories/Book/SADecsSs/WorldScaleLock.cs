using UnityEngine;

[ExecuteInEditMode]
public class WorldScaleLock : MonoBehaviour
{
    public Vector3 targetScale;

    private void Update()
    {
        // I dont know what the fuck is happening here

        if (transform.localScale.x == 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (transform.localScale.y == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }

        if (transform.localScale.z == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        }

        Vector3 currentLossyScale = transform.lossyScale;
        Vector3 scaleRatio = new(
            targetScale.x / currentLossyScale.x,
            targetScale.y / currentLossyScale.y,
            targetScale.z / currentLossyScale.z
        );
        transform.localScale = new Vector3(
            transform.localScale.x * scaleRatio.x,
            transform.localScale.y * scaleRatio.y,
            transform.localScale.z * scaleRatio.z
        );
    }
}
