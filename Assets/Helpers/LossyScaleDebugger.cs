using UnityEngine;

namespace Helpers;

[ExecuteInEditMode]
public class LossyScaleDebugger : MonoBehaviour
{
	[SerializeField]
	private Vector3 lossyScale;

	[SerializeField]
	private Vector3 eulerAngles;

	private void Update()
	{
		lossyScale = transform.lossyScale;
		eulerAngles = transform.eulerAngles;
	}
}
