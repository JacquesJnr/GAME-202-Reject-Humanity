using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyAnimations : MonoBehaviour
{
    public RectTransform monke1, monke2;
    public AnimationCurve curve;
    public float duration;
    public float delay;
    public LeanTweenType easeType;

    private void Start()
    {

    }

    private void OnEnable()
    {
        if(easeType == LeanTweenType.animationCurve)
        {
            LeanTween.rotate(monke1, 15.0f, duration).setDelay(delay).setLoopPingPong().setEase(curve);
            LeanTween.rotate(monke2, 15.0f, duration).setDelay(delay).setLoopPingPong().setEase(curve);
        }
    }
}
