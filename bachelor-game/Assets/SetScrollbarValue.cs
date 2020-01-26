using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetScrollbarValue : MonoBehaviour
{
    public RectTransform windowSize;
    public float windowSizeCompare = 0;
    public Scrollbar scrollbar;


    private void LateUpdate()
    {
        if (windowSizeCompare != windowSize.sizeDelta.y)
        {
            scrollbar.value = 0;
            windowSizeCompare = windowSize.sizeDelta.y;
        }
    }
}
