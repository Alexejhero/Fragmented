using UnityEngine;

/// <summary>
/// For now I'll use this just to test crude movement and animations. Planning to sync with Govo to reuse our old controller
/// </summary>
public class SampleController : MonoBehaviour
{
	[SerializeField]
	private Animator characterAnimator;

	[SerializeField]
	private Rigidbody characterBody;

	private bool isMoving = false;
	private bool isLeft = true;
	private float moveForce = 0.5f;
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
			characterBody.AddForce(new Vector3(-moveForce, 0,0));
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			isLeft = false;
			isMoving = true;
			characterBody.AddForce(new Vector3(moveForce, 0, 0));
		}
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			isMoving = true;
			characterBody.AddForce(new Vector3(0, 0, moveForce));
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			isMoving = true;
			characterBody.AddForce(new Vector3(0, 0, -moveForce));
		}
		//Debug.Log("mov " + isMoving + "  ?? " + isLeft);
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
