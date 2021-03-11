using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationType
{
    Move,
    Rotate,
    Scale,
    ScaleX,
    ScaleY,
    Fade
};

public class UITweener : MonoBehaviour
{
    [SerializeField] private GameObject objectToAnimate;

    [SerializeField] private LeanTweenType inType;
    [SerializeField] private LeanTweenType outType;
    [SerializeField]
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    Vector3 from;
    Vector3 to;

    public bool showOnEnable;
    
    public AnimationCurve curve;
    public UnityEvent onCompletedCallback;

    public void OnEnable()
    {
        
    }

    public void OnComplete()
    {
        if(onCompletedCallback != null)
        {
            onCompletedCallback.Invoke();
        }
    }

    public void OnClose()
    {
        
    }

    public void Show()
    {

    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
