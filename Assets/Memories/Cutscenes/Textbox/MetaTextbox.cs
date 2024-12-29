using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Memories.Cutscenes.Textbox
{
    public sealed class MetaTextbox : TextboxController
    {
        public CanvasGroup canvasGroup;
        public override bool IsShown => canvasGroup.alpha > 0;
        public override async UniTask ShowTextbox()
        {
            if (Mathf.Approximately(canvasGroup.alpha, 1)) return;

            canvasGroup.DOFade(1, 0.5f)
                .SetEase(Ease.InQuad);
            await UniTask.Delay(500);
        }

        public override async UniTask HideTextbox()
        {
            if (Mathf.Approximately(canvasGroup.alpha, 0)) return;

            canvasGroup.DOFade(0, 0.5f)
                .SetEase(Ease.OutQuad);
            await UniTask.Delay(500);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftBracket))
            {
                HideTextbox().Forget();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightBracket))
            {
                ShowTextbox().Forget();
            }
        }
    }
}
