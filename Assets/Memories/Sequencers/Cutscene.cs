using System.Collections;

namespace Memories.Sequencers;

public abstract class Cutscene
{
    public abstract IEnumerator Play();
}
