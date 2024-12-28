using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI neuroText;

    public Image videoHider;

    public VideoPlayer player;

    public VideoClip[] clips;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Global
    public async UniTask Start()
    {
        player.clip = clips[0];
        player.Prepare();

        await UniTask.WaitUntil(() => Input.anyKeyDown);

        await startText.FadeAlpha(0, 1);

        player.Play();
        videoHider.FadeAlpha(0, 0.5f).Forget();

        await UniTask.WaitUntil(() => player.time >= 2.5f);
        videoHider.FadeAlpha(1, 0.5f).Forget();

        await UniTask.Delay(500);

        await neuroText.FadeAlpha(1, 1);
    }
}
