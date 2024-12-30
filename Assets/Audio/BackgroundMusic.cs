using FMODUnity;
using Helpers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Audio;

public enum MusicTrack
{
    Bookshelf,
    Jazzy,
    Retro,
    Pirate,
    Guitars,
}

public sealed class BackgroundMusic : MonoSingleton<BackgroundMusic>
{
    public StudioEventEmitter player;

    protected override void Awake()
    {
        base.Awake();
        this.EnsureComponent(ref player);
        player.Play();
    }

    public MusicTrack GetTrack()
    {
        ParamRef trackParam = player.Params[0];
        Debug.Assert(trackParam.Name == "Track");
        return (MusicTrack)(int)trackParam.Value;
    }

    public void SetTrack(MusicTrack trackEnum)
    {
        player.SetParameter("Track", (int)trackEnum);
    }

    private StudioEventEmitter _nextTrack;
    public float fadeTime = 1f;

    public void SetTrack(StudioEventEmitter newTrack)
    {
        // stop existing fade (if any)
        ResetFade();
        if (_nextTrack)
        {
            _nextTrack.SetParameter("Fade", 0);
            _nextTrack.Stop();
        }
        if (newTrack) newTrack.Play();

        _nextTrack = newTrack;
        _fading = true;
    }

    private void Update()
    {
        UpdateFade();
    }

    private bool _fading;
    private float _fade;
    private void UpdateFade()
    {
        if (!_fading) return;
        _fade += Time.deltaTime / fadeTime;
        if (player)
            player.SetParameter("Fade", 1 - _fade);
        if (_nextTrack)
            _nextTrack.SetParameter("Fade", _fade);
        if (_fade >= 1)
        {
            if (player) player.Stop();
            player = _nextTrack;
            _nextTrack = null;
            ResetFade();
        }
    }

    private void ResetFade()
    {
        _fading = false;
        _fade = 0;
    }
}
