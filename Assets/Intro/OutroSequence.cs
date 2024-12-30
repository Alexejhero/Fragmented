using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;

namespace Intro;

public class OutroSequence : MonoBehaviour
{
    public StudioEventEmitter ambience;

    public Logger logger;

    public TextMeshProUGUI credits;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Global
    public async UniTask Start()
    {
        ambience.Play();
        await logger.RunEnding();

        await UniTask.Delay(1000);

        await UniTask.WaitUntil(() => Input.anyKeyDown);
        await credits.FadeAlpha(1, 1.5f);
        await UniTask.Delay(4000);
        await UniTask.WaitUntil(() => Input.anyKeyDown);
        Application.Quit();
    }
}
