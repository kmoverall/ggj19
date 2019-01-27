using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipPlacer : MonoBehaviour
{
    private RectTransform rect;
    private RectTransform canvas;
    public Vector3 offset;
    [System.NonSerialized]
    public Vector3 basePosition;
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }
    
    void Update()
    {
        Vector2 pivot = Vector2.zero;
        pivot.x = Mathf.Round(basePosition.x / Screen.width);
        pivot.y = Mathf.Round(basePosition.y / Screen.height);

        Vector3 sign = Vector3.zero;
        sign.x = Mathf.Sign(pivot.x - 0.5f);
        sign.y = Mathf.Sign(pivot.y - 0.5f);

        Vector3 pos = basePosition + Vector3.Scale(offset, sign);
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);

        rect.pivot = pivot;
        rect.anchoredPosition = pos;
    }
}
