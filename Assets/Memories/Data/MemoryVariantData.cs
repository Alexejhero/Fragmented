using System.Linq;
using UnityEngine;

namespace Memories.Data;

[CreateAssetMenu(menuName = "Memory/Memory Variant", fileName = "MemoryVariant")]
public sealed class MemoryVariantData : ScriptableObject
{
    public MoodRequirement[] moodRequirements;

    public GameObject gamePrefab;

    public bool MeetsRequirement()
    {
        RouteManager route = RouteManager.Instance;
        return moodRequirements.All(req => req.Matches(route.mood));
    }
}
