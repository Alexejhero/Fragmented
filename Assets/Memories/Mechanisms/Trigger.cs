using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Memories.Mechanisms
{
    public abstract class Trigger<TTarget> : MonoBehaviour
        where TTarget : Component
    {
        public float triggerInterval;
        private float _nextTriggerTime;
        public bool ignoreTriggers;
        private List<Collider> _entered;

        protected virtual void Awake()
        {
            _entered = new List<Collider>(2);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.enabled || !other.gameObject.activeInHierarchy) return;
            if (ignoreTriggers && other.isTrigger) return;
            TTarget target = other.GetComponent<TTarget>();
            if (!target) return;

            if (Time.time < _nextTriggerTime) return;
            _nextTriggerTime = Time.time + triggerInterval;

            _entered.Add(other);
            OnEnter(target);
        }

        public void OnTriggerExit(Collider other)
        {
            if (ignoreTriggers && other.isTrigger) return;
            TTarget target = other.GetComponent<TTarget>();
            if (!target) return;
            int idx = _entered.IndexOf(other);
            if (idx < 0) return;
            _entered.RemoveSwap(idx);

            OnExit(target);
        }

        protected virtual void OnEnter(TTarget target) {}
        protected virtual void OnExit(TTarget target) {}
    }
}
