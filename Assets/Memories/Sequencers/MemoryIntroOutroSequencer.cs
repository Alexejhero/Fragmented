using System.Collections;

namespace Memories.Sequencers;

public abstract class MemoryIntroOutroSequencer
{
    public abstract IEnumerator Intro();
    public abstract IEnumerator Outro();
}
