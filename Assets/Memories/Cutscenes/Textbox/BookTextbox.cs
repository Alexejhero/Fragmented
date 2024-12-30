using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Memories.Cutscenes.Textbox
{
    public sealed class BookTextbox : TextboxController
    {
        public Transform moveableObject;
        public Transform hiddenPosition;
        public Transform activePosition;

        public override bool IsShown => !Mathf.Approximately(moveableObject.position.y, hiddenPosition.position.y);

        public override async UniTask ShowTextbox()
        {
            if (IsShown) return;

            moveableObject.DOMoveY(activePosition.position.y, 0.5f)
                .SetEase(Ease.OutCubic);
            await UniTask.Delay(500);
        }

        public override async UniTask HideTextbox()
        {
            if (!IsShown) return;

            moveableObject.DOMoveY(hiddenPosition.position.y, 0.5f)
                .SetEase(Ease.InCubic);
            await UniTask.Delay(500);
        }

        private void Update()
        {
            if (!Application.isEditor) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Minus))
            {
                HideTextbox().Forget();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Equals))
            {
                ShowTextbox().Forget();
            }
        }
    }
}
