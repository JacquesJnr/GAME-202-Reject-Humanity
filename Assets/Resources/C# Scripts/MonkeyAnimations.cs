using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyAnimations : MonoBehaviour
{
    public RectTransform rectTransform; // The rectTransform of the animated object
    public GameObject fillPrefab;
    public float ScrollX  = 0.5f;
    public float ScrollY  = 0.5f;

    private void OnEnable()
    {
        float offsetX = Time.time * ScrollX;
        float offsetY = Time.time * ScrollY;
        //LeanTween.move(rectTransform, Vector3())

        LeanTween.move(rectTransform, new Vector3(1200.0f, 0, 0), 2f).setDelay(0.5f);
    }

}
