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

        protected void Awake()
        {
            if (!book) book = transform.GetComponentInParent<MemoryBook>();
            if (collider != null) collider.enabled = false;
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
			transform.Rotate(maxLiftAngle * liftDiff);
            lastLift = lift; // book.pageSeparation;

			if (collider != null) collider.enabled = lift > 0.9;
		}
	}
}
