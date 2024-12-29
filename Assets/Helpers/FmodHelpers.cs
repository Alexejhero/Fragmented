using FMODUnity;

namespace Helpers;

public static class FmodHelpers
{
    public static void PlayOneShot(this EventReference evt)
    {
        RuntimeManager.PlayOneShot(evt);
    }
}
