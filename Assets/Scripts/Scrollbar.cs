using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Scrollbar : MonoBehaviour
{
    public GameObject scrollableObject;
    IScrollable scrollable;
    
    public int scrollPoints = 10;
    int value = 0;

    public float scrollbarBackLength = 1260;
    //public float scrollbarCenter = 0;
    public float intervalHeight = 24;
    public float scrollbarThickness = 24;

    public GameObject fullScrollbar;
    public RectTransform scrollbarBack;
    public RectTransform scrollbar;
    public MouseOver scrollbarMouseOver;
    public Image[] scrollbarImages;
    public Color defaultColor;
    public Color highlightColor;

    public float minMouseX;
    public float maxMouseX;

    public bool isHorizontal;
    Vector2 thicknessVector;
    Vector2 lengthVector;

    const float canvasHeight = 1440f;
    const float canvasWidth = 2560f;
    bool mouseDragging = false;
    float mouseOffset;
    float scrollbarBounds;
    float intervalSize;

    public RectTransform scrollbarBackTopEnd;
    public RectTransform scrollbarBackBottomEnd;
    public RectTransform scrollbarTopEnd;
    public RectTransform scrollbarBottomEnd;


    void OnValidate() {
        UpdateScrollbarSize();
    }

    void Awake() {
        scrollable = scrollableObject.GetComponent<IScrollable>();
        UpdateScrollbarSize();
    }

    void Update() {
        if (!mouseDragging) {
            if (scrollbarMouseOver.mouseOver) {
                if (!scrollbarMouseOver.mouseOverLastFrame) {
                    foreach (Image image in scrollbarImages) {
                        image.color = highlightColor;
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    mouseDragging = true;
                    if (isHorizontal) {
                        mouseOffset = (Input.mousePosition.x / Screen.width) - (scrollbar.anchoredPosition.x / canvasWidth);
                    }
                    else {
                        mouseOffset = (Input.mousePosition.y / Screen.height) - (scrollbar.anchoredPosition.y / canvasHeight);
                    }
                }
            }
            else if (scrollbarMouseOver.mouseOverLastFrame) {
                foreach (Image image in scrollbarImages) {
                    image.color = defaultColor;
                }
            }
        }

        if (mouseDragging) {
            float mousePosition;
            if (isHorizontal) {
                mousePosition = Mathf.Clamp((Input.mousePosition.x / Screen.width - mouseOffset) * canvasWidth, -scrollbarBounds, scrollbarBounds);
            }
            else {
                mousePosition = Mathf.Clamp((Input.mousePosition.y / Screen.height - mouseOffset) * canvasHeight, -scrollbarBounds, scrollbarBounds);
            }
            scrollbar.anchoredPosition = lengthVector * mousePosition;

            int closestPosition = 0;
            float closestPositionDistance = Mathf.Infinity;
            for (int i = 0; i <= scrollPoints; i++) {
                float positionCheck = scrollbarBounds - intervalSize * i;
                float dist = Mathf.Abs(positionCheck - mousePosition);
                if (dist < closestPositionDistance) {
                    closestPositionDistance = dist;
                    closestPosition = i;
                }
            }

            int delta = closestPosition - value;
            if (delta != 0) {
                value += delta;
                //slotManager.UpdateScrollOffset(delta);
                if (scrollable != null) {
                    scrollable.UpdateScrollValue(value);
                }
            }

            if (!Input.GetMouseButton(0)) {
                mouseDragging = false;
                if (!scrollbarMouseOver.mouseOver) {
                    foreach (Image image in scrollbarImages) {
                        image.color = defaultColor;
                    }
                }
                UpdateScrollbarPosition();
            }
        }
        else {
            float mouseX = Input.mousePosition.x / Screen.width;
            bool mouseInRange = mouseX >= minMouseX && mouseX <= maxMouseX;

            if (mouseInRange && Input.mouseScrollDelta.y > 0.1f && value > 0) {
                value--;
                UpdateScrollbarPosition();
                if (scrollable != null) {
                    scrollable.UpdateScrollValue(value);
                }
            }
            else if (mouseInRange && Input.mouseScrollDelta.y < -0.1f && value < scrollPoints) {
                value++;
                UpdateScrollbarPosition();
                if (scrollable != null) {
                    scrollable.UpdateScrollValue(value);
                }
            }
        }
    }

    public int GetValue() {
        return value;
    }

    public bool IsAtMaxScroll() {
        return value == scrollPoints;
    }

    public void SetValue(int o) {
        value = Mathf.Clamp(o, 0, scrollPoints);
        UpdateScrollbarPosition();
    }

    public void SetScrollPoints(int l) {
        bool wasAtMax = value == scrollPoints && value > 0;
        scrollPoints = l;
        if (value > scrollPoints || wasAtMax) {
            value = scrollPoints;
        }
        UpdateScrollbarSize();
    }

    void UpdateScrollbarSize() {
        if (scrollPoints <= 0) {
            fullScrollbar.SetActive(false);
        }
        else {
            fullScrollbar.SetActive(true);

            if (isHorizontal) {
                thicknessVector = Vector2.up;
                lengthVector = Vector2.right;
            }
            else {
                thicknessVector = Vector2.right;
                lengthVector = Vector2.up;
            }

            scrollbarBackTopEnd.anchorMin = 0.5f * thicknessVector + lengthVector;
            scrollbarBackTopEnd.anchorMax = 0.5f * thicknessVector + lengthVector;
            scrollbarTopEnd.anchorMin = 0.5f * thicknessVector + lengthVector;
            scrollbarTopEnd.anchorMax = 0.5f * thicknessVector + lengthVector;
            scrollbarBackBottomEnd.anchorMin = 0.5f * thicknessVector;
            scrollbarBackBottomEnd.anchorMax = 0.5f * thicknessVector;
            scrollbarBottomEnd.anchorMin = 0.5f * thicknessVector;
            scrollbarBottomEnd.anchorMax = 0.5f * thicknessVector;

            scrollbarBackTopEnd.anchoredPosition = Vector2.zero;
            scrollbarTopEnd.anchoredPosition = Vector2.zero;
            scrollbarBackBottomEnd.anchoredPosition = Vector2.zero;
            scrollbarBottomEnd.anchoredPosition = Vector2.zero;

            float scrollbarLength = scrollbarBackLength - scrollPoints * intervalHeight;
            scrollbar.sizeDelta = thicknessVector * scrollbarThickness + lengthVector * scrollbarLength;
            scrollbarBack.sizeDelta = thicknessVector * scrollbarThickness + lengthVector * scrollbarBackLength;
            scrollbarBounds = (scrollbarBackLength / 2) - (scrollbarLength / 2);
            intervalSize = (scrollbarBackLength - scrollbarLength) / scrollPoints;

            UpdateScrollbarPosition();
        }
    }

    void UpdateScrollbarPosition() {
        scrollbar.anchoredPosition = lengthVector * (scrollbarBounds - intervalSize * value);
    }
}
