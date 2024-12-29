using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI neuroText;

    public Image videoHider;

    public VideoPlayer player;

    public Logger logger;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Global
    public async UniTask Start()
    {
        player.Prepare();

        await UniTask.WaitUntil(() => Input.anyKeyDown);

        await startText.FadeAlpha(0, 1);

        await UniTask.Delay(3000);

        // player.Play();
        // videoGraphic.FadeAlpha(1, 1f).Forget();

        // await UniTask.WaitUntil(() => player.time >= player.length - 1.5f);
        videoHider.FadeAlpha(1, 1.5f).Forget();

        // await UniTask.Delay(2500);

        await logger.Run();

        // run neuro dialogue

        logger.text.FadeAlpha(0, 1f).Forget();
        neuroText.FadeAlpha(0, 1f).Forget();

        await UniTask.Delay(1000);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
