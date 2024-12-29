using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Popup")]
    public class Popup : MonoBehaviour
    {
        public MemoryBook book;
        public Vector3 maxLiftAngle = new(60, 0, 0);

        public float lastLift;
        // todo: animator
        // public Animator animator;
        [SerializeField]
        private Collider collider;

        [SerializeField]
        private bool initializeCounterRotation = false;

        [SerializeField]
        private bool ignoreBooks = false;

        [SerializeField]
        private bool reverseRotation = false;

        [SerializeField]
        private bool enableLabel = true;

		[SerializeField]
        private TMP_Text tmp;

        [SerializeField]
        private GameObject[] subPivots;

		protected void Awake()
        {
            if (!book && !ignoreBooks) book = transform.GetComponentInParent<MemoryBook>();
            if (collider != null) collider.enabled = false;
            if (initializeCounterRotation) transform.Rotate(reverseRotation ? -maxLiftAngle : maxLiftAngle);
            if (tmp != null) tmp.enabled = false;
		}

        private void Update()
        {
            if (!book) return;
            float lift = book.pageSeparation;
            DoRotate(lift);
        }

        public void DoRotate(float lift)
        {
			// animator can rotate whatever axis
			// animator.SetFloat("PopupLift", lift);

			// X-axis temporary for now
			var liftDiff = lift - lastLift;
			if (Mathf.Approximately(liftDiff, 0)) return;
            Vector3 rotateAmount = (reverseRotation ? maxLiftAngle : -maxLiftAngle) * liftDiff;
			transform.Rotate(rotateAmount);
            DoSubrotate(rotateAmount);
			lastLift = lift; // book.pageSeparation;

			if (collider != null) collider.enabled = lift > 0.9;
            if ((tmp != null) && enableLabel) tmp.enabled = collider.enabled;
		}

        private void DoSubrotate(Vector3 subRotation)
        {
			foreach (GameObject sp in subPivots)
            {
                sp.transform.Rotate(subRotation);
            }
		}
	}
}
