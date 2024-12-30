using FMODUnity;
using Helpers;
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

    public void SetTrack(MusicTrack track)
    {
        player.SetParameter("Track", (int)track);
    }
}
