using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI neuroText;

    public RawImage videoGraphic;
    public Image videoHider;

    public VideoPlayer player;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Global
    public async UniTask Start()
    {
        player.Prepare();

        await UniTask.WaitUntil(() => Input.anyKeyDown);

        await startText.FadeAlpha(0, 1);

        await UniTask.Delay(3000);

        player.Play();
        videoGraphic.FadeAlpha(1, 1f).Forget();

        await UniTask.WaitUntil(() => player.time >= player.length - 1.5f);
        videoHider.FadeAlpha(1, 1.5f).Forget();

        await UniTask.Delay(2500);

        await neuroText.FadeAlpha(1, 1);
    }
}
