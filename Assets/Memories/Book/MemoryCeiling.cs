using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Memories.Book
{
	public class MemoryCeiling : MonoBehaviour
	{
		public float liftThreshold = 0.9f;

		public float minDelay = 0;
		public float maxDelay = 0.2f;

		public float minDropTime = 0.8f;
		public float maxDropTime = 1.2f;

		public float minYOffset = -2.2f;
		public float maxYOffset = -1.8f;

		private readonly List<MemoryCeilingObject> _targets = new();

		private bool _currentlyShown = false;

		private void Awake()
		{
			foreach (MemoryCeilingObject obj in GetComponentsInChildren<MemoryCeilingObject>())
			{
				obj.gameObject.SetActive(false);
				_targets.Add(obj);
			}
		}

		public void DoAction(float lift)
		{
			lift = Mathf.Abs(lift);

			if (lift > liftThreshold && !_currentlyShown)
			{
				DoDrop();
				_currentlyShown = true;
			}
			else if (lift < liftThreshold && _currentlyShown)
			{
				DoRaise();
				_currentlyShown = false;
			}
		}

		private void DoDrop()
		{
			foreach (MemoryCeilingObject obj in _targets)
			{
				obj.DoDrop(minDelay, maxDelay, minDropTime, maxDropTime, minYOffset, maxYOffset).Forget();
			}
		}

		private void DoRaise()
		{
			foreach (MemoryCeilingObject obj in _targets)
			{
				obj.DoRaise(minDropTime, maxDropTime).Forget();
			}
		}
	}
}
