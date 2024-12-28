using System;
using System.Collections;
using Memories.Characters;
using UnityEngine;

namespace Memories
{
    public sealed class Respawnable : MonoBehaviour
    {
        private Vector3 _checkpoint;
        public Action<Respawnable> OnResetBegin;
        public Action<Respawnable> OnResetFinish;

        private void Awake()
        {
            SetCheckpoint(transform.position);
        }
        public void Respawn()
        {
            OnResetBegin?.Invoke(this);
            StartCoroutine(Coroutine());
            return;

            IEnumerator Coroutine()
            {
                Player pl = GetComponent<Player>();
                yield return new WaitForSeconds(pl && pl == Player.ActivePlayer ? 1 : 0);
                transform.position = _checkpoint;
                OnResetFinish?.Invoke(this);
            }
        }
        public void SetCheckpoint(Vector3 pos)
            => _checkpoint = pos;
    }
}
