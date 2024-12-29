using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Helpers;
using Memories.Characters;
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
        public UniTask Show(BookActor actor, string text, CancellationToken ct = default)
        {
            TextboxController textbox = GetByType(actor.textboxType);
            if (!textbox) textbox = bookTextbox;

            return textbox.Show(text, actor.dialogueData, ct);
        }

        private void Update()
        {
            // todo: remove (temp)
            if (UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
                Advance();
        }

        public void Advance()
        {
            GetTopmost().Advance();
        }

        public TextboxController GetTopmost()
        {
            // overlay textbox takes priority
            return metaTextbox.enabled ? metaTextbox
                : bookTextbox.enabled ? bookTextbox
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
    }
}
