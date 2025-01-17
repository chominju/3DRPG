using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;    // 추가
    private RectTransform rectTransform;    // 추가
    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    private Vector2 inputVector;    // 추가
    private bool isInput;    // 추가

    GameObject getPlayer;

    private void Awake()    // 추가
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
        //lever.anchoredPosition = clampedDir;    // 변경


        ControlJoystickLever(eventData);  // 추가
        isInput = true;    // 추가

    }

    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트    // 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        //var inputDir = eventData.position - rectTransform.anchoredPosition- lever.sizeDelta;
        ////lever.anchoredPosition = inputDir;

        //var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        //lever.anchoredPosition = clampedDir;    // 변경

        ControlJoystickLever(eventData);    // 추가
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
        getPlayer.GetComponent<Player>().EndJoystickEnd();
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
