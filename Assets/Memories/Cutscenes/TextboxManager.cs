using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Helpers;
using Memories.Characters;

namespace Memories.Cutscenes;

public class TextboxManager : MonoSingleton<TextboxManager>
{
    private List<Textbox> _textboxes;

    public void Register(Textbox textbox) => _textboxes.Add(textbox);

    public UniTask Show(BookActor actor, string text, CancellationToken ct = default)
    {
        return actor.textbox.Show(text, ct);
    }
}
