using Helpers;
using UnityEngine;

namespace Memories.Characters.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class GravityScaler : MonoBehaviour
    {
        public Rigidbody rb;
        public float scale = 1f;

        private bool _restoreUseGravity;
        private void Awake()
        {
            this.EnsureComponent(ref rb, false);
        }

        private void OnEnable()
        {
            if (!rb.useGravity) return;

            _restoreUseGravity = true;
            rb.useGravity = false;
        }

        private void OnDisable()
        {
            if (!_restoreUseGravity) return;

            _restoreUseGravity = false;
            rb.useGravity = true;
        }

        private void FixedUpdate()
        {
            Vector3 gravity = Physics.gravity * scale;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
