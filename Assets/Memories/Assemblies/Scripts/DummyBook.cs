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
		private bool isAnimating = false;

		private void Awake()
		{
			popups = GetComponentsInChildren<Popup>();
			ceilings = GetComponentsInChildren<MemoryCeiling>();
		}

		private void Update()
		{
			if ((UnityEngine.Input.GetKeyDown(KeyCode.M)) && !isOpen && !isAnimating)
			{
				DoOpen().Forget();
			}else if ((UnityEngine.Input.GetKeyDown(KeyCode.M)) && isOpen && !isAnimating)
			{
				DoOpen(false).Forget();
			}

		}

		private async UniTask DoOpen(bool doOpen = true)
		{
			isAnimating = true;
			await DummyOpen(doOpen);
			if (doOpen) 
			{
				await UniTask.Delay(ceilingDelay);
			}
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

		private async UniTask DummyOpen(bool doOpen = true)
		{
			float lerpval = (doOpen ? 0 : 1);
			while ( (doOpen&&(lerpval < 1)) || (!doOpen && (lerpval > 0)))
			{
				lerpval += (doOpen ? Time.deltaTime : -Time.deltaTime) / openTime;
				UpdatePopups(lerpval);
				await UniTask.Yield();
			}
			isOpen = doOpen;
			isAnimating = false;
		}
	}
}
