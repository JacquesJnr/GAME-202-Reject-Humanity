using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationType
{
    Move,
    RotateZ,
    Scale,
    ScaleX,
    ScaleY,
    Fade
};

public class UITweener : MonoBehaviour
{
    [SerializeField] private GameObject objectToAnimate;
    public AnimationType animationType;
    public LeanTweenType easeType;
    [SerializeField] private LeanTweenType inType;
    [SerializeField] private LeanTweenType outType;
    [SerializeField]
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    [SerializeField] Vector3 from;
    [SerializeField] Vector3 to;

    private LTDescr _tweenObject;

    public bool showOnEnable;
    
    public AnimationCurve curve;
    public UnityEvent onCompletedCallback;

    public void OnEnable()
    {
        Show();
    }    

    public void Show()
    {
        HandleTween();
    }

    public void HandleTween()
    {
        if(objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationType)
        {
            case AnimationType.Scale:
                Scale();
                break;
            case AnimationType.RotateZ:
                Rotate();
                break;
            case AnimationType.Move:
                Move();
                break;
        }

        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);

        if(easeType == LeanTweenType.animationCurve)
        {
            _tweenObject.setEase(curve);
        }

        if (loop)
        {
            _tweenObject.loopCount = int.MaxValue;
        }
        if (pingpong)
        {
            _tweenObject.setLoopPingPong()  ;
        }
    }

    public void Rotate()
    {
        _tweenObject = LeanTween.rotateZ(objectToAnimate, to.z, duration);
    }

    public void Move()
    {
        objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;

        _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
    }

    public void Scale()
    {
        objectToAnimate.GetComponent<RectTransform>().localScale = from;

        _tweenObject = LeanTween.scale(objectToAnimate.GetComponent<RectTransform>(), to, duration);
    }

    public void OnComplete()
    {
        if (onCompletedCallback != null)
        {
            onCompletedCallback.Invoke();
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
