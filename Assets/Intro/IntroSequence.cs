using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI neuroText;

    public RawImage videoGraphic;
    public Image videoHider;

    public VideoPlayer player;

    public Logger logger;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Global
    public async UniTask Start()
    {
        neuroText.maxVisibleCharacters = 0;

        player.Prepare();

        await UniTask.WaitUntil(() => Input.anyKeyDown);

        await startText.FadeAlpha(0, 1);

        await UniTask.Delay(3000);

        player.Play();
        videoGraphic.FadeAlpha(1, 1f).Forget();

        await UniTask.WaitUntil(() => player.time >= player.length - 1.5f);
        videoHider.FadeAlpha(1, 1.5f).Forget();
        DOVirtual.Float(player.GetDirectAudioVolume(0), 0, 1.5f, volume => player.SetDirectAudioVolume(0, volume));

        await UniTask.Delay(2500);

        await logger.Run();

        await UniTask.Delay(1000);

        foreach (char c in neuroText.text)
        {
            neuroText.maxVisibleCharacters++;
            await UniTask.Delay(c switch
            {
                '\n' => 1000,
                ',' => 250,
                ' ' => 100,
                _ => 50
            });
        }

        await UniTask.Delay(1000);

        logger.text.FadeAlpha(0, 1f).Forget();
        neuroText.FadeAlpha(0, 1f).Forget();

        await UniTask.Delay(1000);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
