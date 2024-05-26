using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;    // �߰�
    private RectTransform rectTransform;    // �߰�
    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    private Vector2 inputVector;    // �߰�
    private bool isInput;    // �߰�

    GameObject getPlayer;

    private void Awake()    // �߰�
    {
        rectTransform = GetComponent<RectTransform>();
        getPlayer = GameObject.FindWithTag("Player");
        if (getPlayer == null)
            Debug.Log("Player is not exist");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        //var inputDir = eventData.position - rectTransform.anchoredPosition - lever.sizeDelta;
        ////lever.anchoredPosition = inputDir;


        //var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        //lever.anchoredPosition = clampedDir;    // ����


        ControlJoystickLever(eventData);  // �߰�
        isInput = true;    // �߰�

    }

    // ������Ʈ�� Ŭ���ؼ� �巡�� �ϴ� ���߿� ������ �̺�Ʈ    // ������ Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        //var inputDir = eventData.position - rectTransform.anchoredPosition- lever.sizeDelta;
        ////lever.anchoredPosition = inputDir;

        //var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        //lever.anchoredPosition = clampedDir;    // ����

        ControlJoystickLever(eventData);    // �߰�
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition - lever.sizeDelta;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputVector = clampedDir / leverRange;

        //Debug.Log("inputVector : " + inputVector.x + " /// " + inputVector.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        getPlayer.GetComponent<PlayerMove>().EndJoystickEnd();
    }

    void Update()
    {
        //if (isInput)
        //{
        //    getPlayer.GetComponent<PlayerMove>().JoystickMove(inputVector);
        //}
    }

    public bool GetIsInput()
    {
        return isInput;
    }

    public Vector2 GetinputVector()
    {
        return inputVector;
    }
}
