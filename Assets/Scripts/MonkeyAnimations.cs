using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyAnimations : MonoBehaviour
{
    private RectTransform rectTransform; // The rectTransform of the animated object

    public AnimationCurve curve;
    public float duration;
    public float delay;
    public float rotateAmount = 15f;
    public LeanTweenType easeType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if(easeType == LeanTweenType.animationCurve)
        {
            LeanTween.rotate(rectTransform, rotateAmount, duration).setDelay(delay).setLoopPingPong().setEase(curve);
        }
    }
}
