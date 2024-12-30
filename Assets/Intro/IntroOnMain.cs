using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroOnMain : MonoBehaviour
{
    public TextMeshProUGUI neuroTextMiddle;
    public Image videoHider;
    public MainSceneScript mainSceneScript;

    // ReSharper disable once Unity.IncorrectMethodSignature UnusedMember.Local
    private async UniTask Start()
    {
        neuroTextMiddle.FadeAlpha(1, 2).Forget();
        await UniTask.Delay(2000);

        videoHider.FadeAlpha(0, 2f).Forget();
        await UniTask.Delay(1500);
        neuroTextMiddle.FadeAlpha(0, 1f).Forget();

        await UniTask.Delay(1000);

        mainSceneScript.busy = false;
        gameObject.SetActive(false);
    }
}
