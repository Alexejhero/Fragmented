using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Helpers;
using UnityEngine;

namespace Memories.Cutscenes.Textbox
{
    public enum TextboxType
    {
        Book,
        Meta
    }
    public sealed class TextboxManager : MonoSingleton<TextboxManager>
    {
        public BookTextbox bookTextbox;
        public MetaTextbox metaTextbox;
        public UniTask Show(DialogueActorData actor, string text, int dropdownAt = -1, CancellationToken ct = default)
        {
            TextboxController textbox = GetByType(actor.textboxType);
            if (!textbox) textbox = bookTextbox;

            return textbox.Show(text, actor, dropdownAt, ct);
        }

        public void Clear(DialogueActorData actor)
        {
            TextboxController textbox = GetByType(actor.textboxType);
            if (!textbox) textbox = bookTextbox;

            textbox.Clear();
        }

        private void Update()
        {
            // todo: remove (temp)
            if (UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
                Advance();
        }

        public void Advance()
        {
            TextboxController topmost = GetTopmost();
            if (topmost) topmost.Advance();
        }

        public TextboxController GetTopmost()
        {
            // overlay textbox takes priority
            return metaTextbox.IsShown ? metaTextbox
                : bookTextbox.IsShown ? bookTextbox
                : null;
        }

        public TextboxController GetByType(TextboxType type)
        {
            return type switch
            {
                TextboxType.Book => bookTextbox,
                TextboxType.Meta => metaTextbox,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        public async UniTask HideTopmost()
        {
            TextboxController topmost = GetTopmost();
            if (topmost) await topmost.HideTextbox();
        }
    }
}
