using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Memories.Cutscenes.Textbox
{
    public abstract class TextboxController : MonoBehaviour
    {
        public TMP_Text tmp;
        [Tooltip("Characters revealed per second")]
        public int speed = 16;

        [NonSerialized]
        public int LastCharacterDropdown = -1;

        // delay equivalent to x characters
        public int spacersPauseCharacters = 5;
        public int sentencePauseCharacters = 8;
        public int ellipsisPauseCharacters = 15;

        public abstract UniTask ShowTextbox();
        public abstract UniTask HideTextbox();
        public abstract bool IsShown { get; }

        public void Clear()
        {
            LastCharacterDropdown = -1;
            tmp.text = "";
        }

        public async UniTask Show(string text, DialogueActorData actor, int dropdownAt = -1, CancellationToken ct = default)
        {
            Clear();
            await ShowTextbox();
            await DisplayText(text, actor, dropdownAt, ct);
            await WaitForAdvance(ct);
        }

        public async UniTask DisplayText(string text, DialogueActorData actor, int dropdownAt = -1, CancellationToken ct = default)
        {
            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            try
            {
                LastCharacterDropdown = dropdownAt;
                tmp.font = actor.fontAsset;
                tmp.fontSizeMin = actor.minFontSize;
                tmp.fontSizeMax = actor.maxFontSize;
                tmp.color = actor.textColor;
                tmp.text = text;
                tmp.maxVisibleCharacters = 0;
                // ReSharper disable once AccessToDisposedClosure
                WaitForAdvance(linkedCts.Token) // skip and display all
                    .ContinueWith(linkedCts.Cancel).Forget();
                bool skipped = await TickCharacters(text, actor.talkNoise, linkedCts.Token).SuppressCancellationThrow();
                if (skipped)
                {
                    tmp.maxVisibleCharacters = text.Length;
                }
            }
            finally
            {
                linkedCts.Cancel();
                linkedCts.Dispose();
            }
        }

        private async UniTask TickCharacters(string text, EventReference talkNoise, CancellationToken ct = default)
        {
            bool haveTalkNoise = !talkNoise.IsNull;
            // play noise every other char (every char is too much)
            bool played = false;
            float delayPerCharacter = 1f / speed;

            for (int i = 0; i < text.Length; i++)
            {
                tmp.maxVisibleCharacters = i + 1;

                char c = text[i];
                if (char.IsWhiteSpace(c)) continue;

                float delayCharacters = c switch
                {
                    ',' => spacersPauseCharacters,
                    // " - "
                    '-' when i > 0 && char.IsWhiteSpace(text[i-1]) && i < text.Length - 1 && char.IsWhiteSpace(text[i + 1])
                        => spacersPauseCharacters,
                    // "..." with more delay with each subsequent '.' (e.g. 2x with "....", 3x with ".....", etc)
                    '.' when i>1 && text[i-1] == '.' && text[i-2] == '.'
                        => ellipsisPauseCharacters,
                    '.' or '!' or '?' when i+1 < text.Length && char.IsWhiteSpace(text[i+1])
                        => sentencePauseCharacters,
                    _ => 1,
                };

                if (haveTalkNoise && !char.IsPunctuation(c) && (played = !played))
                    RuntimeManager.PlayOneShot(talkNoise);

                float delay = delayPerCharacter * delayCharacters;
                await UniTask.Delay(Mathf.RoundToInt(1000 * delay), cancellationToken: ct);
            }
        }

        private UniTaskCompletionSource _advanceSource = new();
        public UniTask WaitForAdvance(CancellationToken ct = default) => _advanceSource.Task.AttachExternalCancellation(ct);

        public void Advance()
        {
            if (!IsShown) return;

            Debug.Log("Advance");
            // need to swap _advanceSource before TrySetResult() invokes its callbacks
            UniTaskCompletionSource oldSource = _advanceSource;
            _advanceSource = new();
            oldSource.TrySetResult();
        }

        // input system, triggers when clicking anywhere (or pressing the key bound to Submit)
        [UsedImplicitly]
        public void OnClick(InputValue val)
        {
            if (!IsShown) return;

            Debug.Log("Clicked");
            Advance();
        }
    }
}
