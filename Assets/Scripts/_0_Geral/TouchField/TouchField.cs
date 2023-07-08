using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchField : MonoSingleton<TouchField>, IPointerDownHandler, IPointerUpHandler
{
    public Action onPointerDownEvent;
    public Action onPointerUpEvent;

    [Header("Primeiro toque")]
    [SerializeField] private TouchData firstTouchData;

    [Header("Segundo toque")]
    [SerializeField] private TouchData secondTouchData;

    [Header("Movimento de Pinça")]
    [SerializeField] private float pinchDistance;
    [SerializeField] private float oldPinchDistance;

    [SerializeField, ReadOnly] private bool isPinching;

    [SerializeField, ReadOnly] private float delayAfterpinching = 1f;
    [SerializeField, ReadOnly] private float currentDelayAfterpinching;

    [SerializeField] private bool debug = true;

    protected override void InitializeBehaviour()
    {
        firstTouchData = new TouchData();
        secondTouchData = new TouchData();
    }

    protected override void FinishBehaviour()
    {
    }

    private void OnValidate()
    {
        if (GetComponentInChildren<TMPro.TMP_Text>() == null)
        {
            debug = false;
            return;
        }

        if (debug)
        {
            GetComponentInChildren<TMPro.TMP_Text>().enabled = true;
        }
        else
        {
            GetComponentInChildren<TMPro.TMP_Text>().enabled = false;
        }
    }

    private void Debug()
    {
        //Debug
        string debug = string.Empty;

        debug += $"Touch suporte: {Input.touchSupported} : {Input.touchCount}\n";
        debug += $"Toque {firstTouchData.ID} : {firstTouchData.isPressing} :: {firstTouchData.touchDirection} : {firstTouchData.oldTouchPosition}\n";
        debug += $"Toque {secondTouchData.ID} : {secondTouchData.isPressing} :: {secondTouchData.touchDirection} : {secondTouchData.oldTouchPosition}\n";
        debug += $"Pinça : {firstTouchData.isPressing && secondTouchData.isPressing} : {pinchDistance} :: {oldPinchDistance}";


        GetComponentInChildren<TMPro.TMP_Text>().text = debug;
        //Debug
    }

    void Update()
    {
        if (firstTouchData.isPressing && currentDelayAfterpinching <= 0)
        {
            firstTouchData.touchDirection = (Input.touchCount > 0 ? Input.touches.ToList().First().position : (Vector2)Input.mousePosition) - firstTouchData.oldTouchPosition;

            firstTouchData.oldTouchPosition = (Input.touchCount > 0 ? Input.touches.ToList().First().position : (Vector2)Input.mousePosition);
        }
        else
        {
            firstTouchData.oldTouchPosition = (Input.touchCount > 0 ? Input.touches.ToList().First().position : (Vector2)Input.mousePosition);
        }

        if (secondTouchData.isPressing && currentDelayAfterpinching <= 0)
        {
            secondTouchData.touchDirection = (Input.touchCount > 0 ? Input.touches.ToList().Last().position : (Vector2)Input.mousePosition) - secondTouchData.oldTouchPosition;

            secondTouchData.oldTouchPosition = (Input.touchCount > 0 ? Input.touches.ToList().Last().position : (Vector2)Input.mousePosition);
        }
        else
        {
            secondTouchData.oldTouchPosition = (Input.touchCount > 0 ? Input.touches.ToList().Last().position : (Vector2)Input.mousePosition);
        }

#if UNITY_EDITOR
        pinchDistance = Input.GetAxis("Mouse ScrollWheel") * 15f;

        isPinching = !Mathf.Approximately(pinchDistance, 0);
#else
        if (firstTouchData.isPressing && secondTouchData.isPressing)
        {
            isPinching = true;

            pinchDistance = Vector2.Distance(firstTouchData.oldTouchPosition, secondTouchData.oldTouchPosition) - oldPinchDistance;
            oldPinchDistance = Vector2.Distance(firstTouchData.oldTouchPosition, secondTouchData.oldTouchPosition);

            firstTouchData.touchDirection = firstTouchData.oldTouchPosition;
            secondTouchData.touchDirection = secondTouchData.oldTouchPosition;
        }
        else
        {
            isPinching = false;

            oldPinchDistance = 0;
        }
#endif

        if(isPinching)
        {
            currentDelayAfterpinching = delayAfterpinching;
        }
        else
        {
            currentDelayAfterpinching -= Time.deltaTime;
            
            if (currentDelayAfterpinching < 0) 
                currentDelayAfterpinching = 0;
        }
    }

    public float Horizontal()
    {
        if (currentDelayAfterpinching > 0) return 0f;

        if(firstTouchData.isPressing && !secondTouchData.isPressing)
        {
            return firstTouchData.touchDirection.x;
        }
        else if(!firstTouchData.isPressing && secondTouchData.isPressing)
        {
            return secondTouchData.touchDirection.x;
        }
        else
        {
            return 0f;
        }
    } 
    public float Vertical()
    {
        if (currentDelayAfterpinching > 0)
        {
            return 0f;
        }

        if (firstTouchData.isPressing && !secondTouchData.isPressing)
        {
            return firstTouchData.touchDirection.y;
        }
        else if (!firstTouchData.isPressing && secondTouchData.isPressing)
        {
            return secondTouchData.touchDirection.y;
        }
        else
        {
            return 0f;
        }
    }

    public float PinchDistance => pinchDistance;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentDelayAfterpinching > 0) return;

        GameController.Instance.SetInativo(false);

        if (!firstTouchData.isPressing)
        {
            firstTouchData.touchDirection = eventData.position;
            firstTouchData.oldTouchPosition = firstTouchData.touchDirection;
            firstTouchData.ID = eventData.pointerId;
            firstTouchData.isPressing = true;
        }
        else if (!secondTouchData.isPressing)
        {
            secondTouchData.touchDirection = eventData.position;
            secondTouchData.oldTouchPosition = secondTouchData.touchDirection;
            secondTouchData.ID = eventData.pointerId;
            secondTouchData.isPressing = true;
        }

        onPointerDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameController.Instance.SetInativo(false);

        int pointerID = eventData.pointerId;

        if (pointerID == firstTouchData.ID)
        {
            firstTouchData = new TouchData();

            pinchDistance = 0;

            secondTouchData.isPressing = false;
        }
        else if (pointerID == secondTouchData.ID)
        {
            secondTouchData = new TouchData();

            pinchDistance = 0;

            firstTouchData.isPressing = false;
        }

        onPointerUpEvent?.Invoke();
    }
}