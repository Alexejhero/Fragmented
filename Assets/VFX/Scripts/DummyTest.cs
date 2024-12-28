using UnityEngine;

namespace VFX.Scripts;
public class DummyTest : MonoBehaviour
{
    public AnimationCurve curve;
    // Start is called before the first frame update
    public void DoTHeTHieng()
    {
        EffectManager instance = EffectManager.Instance;
        instance.FullScreenFadeIn(Color.black,2f, curve);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
