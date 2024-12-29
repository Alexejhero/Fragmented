using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memories.Book
{
	public class MemoryCeiling : MonoBehaviour
	{
		[SerializeField]
		private float dropQuantity = 2.0f;

		[SerializeField]
		private float dropTime = 1.0f;

		private Vector3 start;
		private Vector3 end;

		private Renderer[] _rends;

		private void Awake()
		{
			start = transform.position;
			end = start;
			end.y -= dropQuantity;

			_rends = GetComponentsInChildren<Renderer>();
			ToggleRend(false);
		}

		public void DoDrop()
		{
			DropCeiling().Forget(); //bounce anim?
		}

		private async UniTask DropCeiling(float lerpstart = 0)
		{
			ToggleRend(true);
			float lerpval = lerpstart;
			while (lerpval < 1)
			{
				lerpval += Time.deltaTime / dropTime;
				transform.position = Vector3.Slerp(start, end, lerpval);
				await UniTask.Yield();
			}
		}

		private void ToggleRend(bool toggle = true)
		{
			foreach (Renderer mr in _rends)
			{
				mr.enabled = toggle;
			}
		}
	}
}