using UnityEngine;

/// <summary>
/// For now I'll use this just to test crude movement and animations. Planning to sync with Govo to reuse our old controller
/// </summary>
public class SampleController : MonoBehaviour
{
	[SerializeField]
	private Animator characterAnimator;

	private bool isMoving = false;
	private bool isLeft = true;

	private void Awake()
	{
		//characterAnimator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		isMoving = false;
		if (UnityEngine.Input.GetKey(KeyCode.A))
		{
			isLeft = true;
			isMoving = true;
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			isLeft = false;
			isMoving = true;
		}
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			isMoving = true;
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			isMoving = true;
		}
		Debug.Log("mov " + isMoving + "  ?? " + isLeft);
		characterAnimator.SetBool("isLeft", isLeft);
		characterAnimator.SetBool("isMoving", isMoving);

		if (isLeft)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			transform.localScale = Vector3.one;
		}
	}

	private void PlayerReset()
	{
		characterAnimator.SetBool("isLeft", true);
		characterAnimator.SetBool("isMoving", false);
	}

}
