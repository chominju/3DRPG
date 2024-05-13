using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;    // 추가
    private RectTransform rectTransform;    // 추가

    private void Awake()    // 추가
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        var inputDir = eventData.position - rectTransform.anchoredPosition - lever.sizeDelta;
        lever.anchoredPosition = inputDir;
        Debug.Log("eventData.position : " + eventData.position.x+ " / " + eventData.position.y);
        Debug.Log("rectTransform.anchoredPosition : " + rectTransform.anchoredPosition.x + " / " + rectTransform.anchoredPosition.y);
        Debug.Log("lever.anchoredPosition.anchoredPosition : " + lever.anchoredPosition.x + " / " + lever.anchoredPosition.y);
        Debug.Log("rectTransform.sizeDelta : " + rectTransform.sizeDelta.x + " / " + rectTransform.sizeDelta.y);
    }

    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트    // 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        var inputDir = eventData.position - rectTransform.anchoredPosition- lever.sizeDelta;
        lever.anchoredPosition = inputDir;
        Debug.Log("eventData.position : " + eventData.position.x + " / " + eventData.position.y);
        Debug.Log("rectTransform.anchoredPosition : " + rectTransform.anchoredPosition.x + " / " + rectTransform.anchoredPosition.y);
        Debug.Log("lever.anchoredPosition.anchoredPosition : " + lever.anchoredPosition.x + " / " + lever.anchoredPosition.y);
        Debug.Log("rectTransform.sizeDelta : " + rectTransform.sizeDelta.x + " / " + rectTransform.sizeDelta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
    }
}
