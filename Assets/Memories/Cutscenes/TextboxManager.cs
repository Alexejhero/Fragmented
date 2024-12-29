using System.Threading;
using Cysharp.Threading.Tasks;
using Helpers;
using Memories.Characters;

namespace Memories.Cutscenes;

public sealed class TextboxManager : MonoSingleton<TextboxManager>
{
    public UniTask Show(BookActor actor, string text, CancellationToken ct = default)
    {
        return actor.textbox.Show(text, actor.dialogueData, ct);
    }
}
