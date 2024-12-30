using System.Collections;
using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Memories.Book;
using Memories.Characters.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using VFX.Last_scene;

public class Outro : MonoBehaviour
{
    [FormerlySerializedAs("destroyList")] public UnityEngine.Object[] disableList;

    private bool _started = false;
    public LastEffect eff;
    public void StartOutro()
    {
        if (_started) return;
        _started = true;

        OutroSequence().Forget();

    }

    private async UniTask OutroSequence()
    {
        foreach (Object obj in disableList)
        {
            if (obj is GameObject go)
            {
                go.SetActive(false);
            }
            else if (obj is MonoBehaviour mono)
            {
                mono.enabled = false;
            }
            else if (obj is Collider col)
            {
                col.enabled = false;
            }
            else if (obj is Renderer rend)
            {
                rend.enabled = false;
            }
        }

        eff.Play();
        // await UniTask.Delay(3000);
        await UniTask.Delay((int) (eff.DurationSeconds * 1000));
        await UniTask.Delay(2000);

        DOVirtual.Float(1, 0, 2, v => BackgroundMusic.Instance.player.SetParameter("Fade", v));

        await UniTask.Delay(2000);

        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
