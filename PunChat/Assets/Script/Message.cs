using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Message : MonoBehaviour
{
    [SerializeField] Color myColor;

    [SerializeField] Color friendColor;

    [SerializeField] Text message;

    [SerializeField] Image image;

    #region PRIAVATE FIELDS

    Vector2 leftMidAnchor = new Vector2(0,0.5f);

    Vector2 rightMidAnchor = new Vector2(1, 0.5f);

    Vector2 rightMidPosition = new Vector2(-850, 0);

    RectTransform rectTransform;

    #endregion

    public void ShowMessage(object _message, bool isMine)
    {
        rectTransform = image.GetComponent<RectTransform>();

        message.text = _message.ToString();

        image.color = isMine ? myColor : friendColor;

        rectTransform.anchoredPosition = isMine ? rightMidPosition : Vector2.zero;

        rectTransform.anchorMin = rectTransform.anchorMax = isMine ? rightMidAnchor : leftMidAnchor;

    }
}
