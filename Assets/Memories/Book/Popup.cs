using TMPro;
using UnityEngine;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Popup")]
    public class Popup : BasePopup
    {
        public MemoryBook book;
        public Vector3 maxLiftAngle = new(60, 0, 0);

        public float lastLift;
        // todo: animator
        // public Animator animator;
        [SerializeField]
        private new Collider collider;

        [SerializeField]
        private bool initializeCounterRotation = false;

        [SerializeField]
        private bool ignoreBooks = false;

        public bool reverseRotation = false;

        [SerializeField]
        private bool enableLabel = true;

		[SerializeField]
        private TMP_Text tmp;

        [SerializeField]
        private GameObject[] subPivots;

        [SerializeField]
        private float speed = 1;

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

        public override void DoRotate(float lift)
        {
	        lift = Mathf.Abs(lift); // we don't care about the direction
	        lift = Mathf.Pow(lift, speed);

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
