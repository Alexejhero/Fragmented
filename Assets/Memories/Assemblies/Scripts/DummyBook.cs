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
		private MemoryCeiling[] ceilings;

		[SerializeField]
		private float openTime=2f;

		[SerializeField]
		private int ceilingDelay = 800; //milliseconds

		private bool isOpen = false;

		private void Awake()
		{
			popups = GetComponentsInChildren<Popup>();
			ceilings = GetComponentsInChildren<MemoryCeiling>();
		}

		private void Update()
		{
			if ((UnityEngine.Input.GetKeyDown(KeyCode.M)) && !isOpen)
			{
				isOpen = true;
				DoOpen().Forget();
			}
		}

		private async UniTask DoOpen()
		{
			await DummyOpen();
			await UniTask.Delay(ceilingDelay);
			// drop ceilings after opening
			DropCeilings();
		}

		private void UpdatePopups(float lift)
		{
			foreach (Popup mp in popups)
			{
				mp.DoRotate(lift);
			}
		}

		private void DropCeilings()
		{
			foreach (MemoryCeiling mc in ceilings)
			{
				mc.DoDrop();
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
