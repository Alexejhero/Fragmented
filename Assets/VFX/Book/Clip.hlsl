void Clip_float(float mask, float clipTreshold, float dissolveThick, out float result)
{
    
    float clipOffset = clipTreshold + dissolveThick + 0.001;
    result = distance(mask, clipOffset);
    //result = (dissolveThick - ((clipTreshold - mask) + (clipOffset - mask))) * 1 / ( dissolveThick * 2);
};
