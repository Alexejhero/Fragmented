using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Memories.Book
{
	public class DummyBook : MonoBehaviour
	{
		[SerializeField]
		private Popup[] popups;

		[SerializeField]
		private float openTime=2f;

		private bool isOpen = false;

		private void Awake()
		{
			popups = GetComponentsInChildren<Popup>();
		}

		private void Update()
		{
			if ((UnityEngine.Input.GetKeyDown(KeyCode.O)) && !isOpen)
			{
				DummyOpen().Forget();
			}
		}

		private void UpdatePopups(float lift)
		{
			foreach (Popup mp in popups)
			{
				mp.DoRotate(lift);
			}
		}

		private async UniTask DummyOpen()
		{
			float lerpval = 0;
			while (lerpval < 1)
			{
				lerpval += Time.deltaTime / openTime;
				UpdatePopups(lerpval);
				await UniTask.Yield();
			}
		}
	}
}