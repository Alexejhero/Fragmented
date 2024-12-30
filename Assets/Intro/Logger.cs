using Cysharp.Threading.Tasks;
using FMODUnity;
using Helpers;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public EventReference beep;

    private int _hour = 21;
    private int _minute = 15;
    private float _second = 47;

    private void Update()
    {
        _second += Time.deltaTime;
        if (_second >= 60)
        {
            _second -= 60;
            _minute++;
            if (_minute >= 60)
            {
                _minute -= 60;
                _hour++;
                if (_hour >= 24)
                {
                    _hour -= 24;
                }
            }
        }
    }

    public async UniTask Run()
    {
        LogWarning("stream ended (timer)");
        Log("running post-stream diagnosis...");
        await UniTask.Delay(500);
        Log("cpu usage: 37%");
        await UniTask.Delay(100);
        Log("cpu temp: 95°C");
        LogWarning("cpu overheat, check radiator for punctures");
        await UniTask.Delay(500);
        Log("ram usage: 68GB/128GB");
        LogWarning("potential memory leak detected");
        await UniTask.Delay(500);
        Log("gpu usage: 56%");
        await UniTask.Delay(100);
        Log("gpu temp: 51°C");
        await UniTask.Delay(500);
        Log("disk space: 31.99TB/32TB (99.99%)");
        LogError("disk full, queue cleanup");
        await UniTask.Delay(500);
        Log("no ndi errors reported");
        await UniTask.Delay(100);
        LogError("diagnosis resulted in 1 error: disk full, queue cleanup");
        await UniTask.Delay(2000);
    }

    private void Log(string message)
    {
        LogInternal("white", message);
    }

    private void LogWarning(string message)
    {
        LogInternal("yellow", message);
    }

    private void LogError(string message)
    {
        LogInternal("red", message);
        beep.PlayOneShot();
    }

    private void LogInternal(string color, string message)
    {
        text.text += $"<color={color}>[{_hour:D2}:{_minute:D2}:{(int)_second:D2}] {message}</color>\n";
    }
}
