using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCharging = false;
    public static float originalWidth = 100.0f;
    float width = 1.0f;
    public static float widthPerSecond = 0;
    public static float maxWidth = 900.0f;
    private RectTransform rec;

    private void Start()
    {
        rec = GetComponent<RectTransform>();
        width = originalWidth;
        widthPerSecond = BarController.computeIncrement();
    }
    // Update is called once per frame
    private static float computeIncrement()
    {
        print(maxWidth);
        print(originalWidth);
        print(BallController.MAX_TIME);
        return (maxWidth - originalWidth)/ BallController.MAX_TIME;
    }

    void Update()
    {
        if (!isCharging)
            return;
        if (width >= maxWidth)
        {
            width = maxWidth;
            isCharging = false;
        }
        width += widthPerSecond*Time.deltaTime;
        print(widthPerSecond);
        print(Time.deltaTime);
        rec.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    internal void reset()
    {
        isCharging = false;
        this.width = originalWidth;
        rec.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalWidth);
    }
}
