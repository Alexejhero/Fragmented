void Offset_float(float2 uv, float mul, out float result)
{
    float2 offsets[] = {
        float2(1,0),
        float2(0,1),
        float2(-1,0),
        float2(0,-1),
    };
    float val = 0;
    for (int i = 0; i < 4; i++)
    {
        val += LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv + offsets[i] * mul), _ZBufferParams);
        if( uv.x > 0.5) clip(1);
    }
    result = val / 4.0f;
};
