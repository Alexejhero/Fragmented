using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class NeuroSubtitles : MonoBehaviour
{
    [Serializable]
    public class Subtitle
    {
        public float time;
        [TextArea] public string text;
    }

    public TextMeshProUGUI text;
    public VideoPlayer player;
    public Subtitle[] subtitles;

    private int _index;

    private void Update()
    {
        if (_index < subtitles.Length && player.time >= subtitles[_index].time)
        {
            text.text = subtitles[_index].text;
            _index++;
        }
    }
}
