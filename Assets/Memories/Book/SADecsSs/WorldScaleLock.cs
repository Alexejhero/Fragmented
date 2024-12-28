using UnityEngine;

[ExecuteInEditMode]
public class WorldScaleLock : MonoBehaviour
{
    private Vector3 _targetScale;

    private void Awake()
    {
        _targetScale = transform.lossyScale;
    }

    private void Update()
    {
        // Adjust localScale so that lossyScale matches
        Vector3 currentLossyScale = transform.lossyScale;
        Vector3 scaleRatio = new Vector3(
            _targetScale.x / currentLossyScale.x,
            _targetScale.y / currentLossyScale.y,
            _targetScale.z / currentLossyScale.z
        );
        transform.localScale = new Vector3(
            transform.localScale.x * scaleRatio.x,
            transform.localScale.y * scaleRatio.y,
            transform.localScale.z * scaleRatio.z
        );
    }
}
