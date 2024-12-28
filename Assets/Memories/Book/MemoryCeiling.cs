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

		private void Awake()
		{
			start = transform.position;
			end = start;
			end.y -= dropQuantity;
		}

		public void DoDrop()
		{
			DropCeiling().Forget(); //bounce anim?
		}

		private async UniTask DropCeiling(float lerpstart = 0)
		{
			float lerpval = lerpstart;
			while (lerpval < 1)
			{
				lerpval += Time.deltaTime / dropTime;
				transform.position = Vector3.Slerp(start, end, lerpval);
				await UniTask.Yield();
			}
		}
	}
}