using UnityEngine;

namespace Memories.Book
{
    [AddComponentMenu("Memory/Book")]
    public class MemoryBook : MonoBehaviour
    {
        public GameObject left;
        public GameObject right;

        public float pageSeparation;

        private void Update()
        {
            float absDiff = Mathf.Abs(left.transform.localEulerAngles.y - right.transform.localEulerAngles.y);
            float degreeDiff = absDiff % 360;
            const float ONE_OVER_180 = 1f / 180f; // multiplication is faster than division;
            pageSeparation = degreeDiff * ONE_OVER_180;
        }
    }
}
