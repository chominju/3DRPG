using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;    // �߰�
    private RectTransform rectTransform;    // �߰�

    private void Awake()    // �߰�
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

    // ������Ʈ�� Ŭ���ؼ� �巡�� �ϴ� ���߿� ������ �̺�Ʈ    // ������ Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����    
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
