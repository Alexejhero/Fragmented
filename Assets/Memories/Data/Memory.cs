using System.Linq;
using UnityEngine;

namespace Memories.Data;

public sealed class Memory : MonoBehaviour
{
    public MemoryData data;
    public MemoryVariantData GetActiveVariant()
    {
        return data.variants.First(v => v.moodRequirements.All(r => r.Matches(RouteManager.Instance.mood)));
    }
}
