using System.Collections;
using Audio;
using FMODUnity;
using Memories.Characters.Movement;
using UnityEngine;

namespace Memories.Characters
{
    public sealed class Player : MonoBehaviour
    {
        public static Player ActivePlayer;

        public Respawnable respawn;
        public PlayerController controller;
        // public Inventory inventory;
        public Rigidbody2D rb;
        [HideInInspector] public bool dying;
        public StudioEventEmitter deathSound;

        // public bool ForceFacingFront { get; set; }
        public bool Locked => false;

        public bool IsGrounded => controller.groundTracker.IsGrounded;
        public bool IsRecentlyGrounded => controller.groundTracker.IsRecentlyGrounded;

        private SpriteRenderer[] _renderers;

        private void Awake()
        {
            AudioSystem.PauseSfx(false);
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            respawn.OnResetBegin += _ =>
            {
                if (dying) return;

                if (deathSound)
                {
                    deathSound.Play();
                    deathSound.SetParameter("Is Active Player", this == ActivePlayer ? 1 : 0);
                }

                if (this != ActivePlayer) return;

                // PauseMenuBehaviour.Instance.Close();
                // PauseMenuBehaviour.Instance.canToggle = false;
                rb.simulated = false;
                controller.enabled = false;
                dying = true;
                // EffectsManager.Instance.PlayEffect(EffectsManager.Effects.death, 1f);
            };
            respawn.OnResetFinish += _ =>
            {
                // if (inventory.item)
                // {
                //     Item item = inventory.item;
                //     inventory.DetachItem();
                //     Respawnable itemRespawn = item.GetComponent<Respawnable>();
                //     if (itemRespawn) itemRespawn.Respawn();
                // }

                if (this != ActivePlayer) return;

                // PauseMenuBehaviour.Instance.canToggle = true;
                rb.simulated = true;
                controller.enabled = true;
                dying = false;
                StartCoroutine(OnRespawn());
            };
        }

        public void OnEnable()
        {
            // todo 2d camera
            Camera.main!.GetComponent<CameraController>().target = transform;
            ActivePlayer = this;
            ToggleMovement(true);
            SetSortOrder(1);
        }

        public void OnDisable()
        {
            ToggleMovement(false);
            SetSortOrder(-1);
        }

        private void SetSortOrder(int order)
        {
            foreach (SpriteRenderer spriteRenderer in _renderers)
                spriteRenderer.sortingOrder = order;
        }

        public void ToggleMovement(bool active)
        {
            controller.canMove = active;
            controller.canJump = active;
        }

        private IEnumerator OnRespawn()
        {
            yield return null;
        }
    }
}
