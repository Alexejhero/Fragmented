using UnityEngine;

namespace Memories.Characters.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Noclip : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Characters.Player player;
        public PlayerController controller;
        public float flySpeed;
        private void Start()
        {
            if (!Application.isEditor) Destroy(this);
        }
        private void OnEnable()
        {
            rb.isKinematic = true;
        }

        private void OnDisable()
        {
            rb.isKinematic = false;
        }

        private void Update()
        {
            if (player != Characters.Player.ActivePlayer) return;
            rb.velocity = Vector2.zero;
            Vector2 movement = flySpeed * Time.deltaTime * controller.MoveInput;
            transform.position += (Vector3)movement;
        }

        public void OnNoclip()
        {
            if (player != Characters.Player.ActivePlayer) return;
            enabled = !enabled;
        }
    }
}
