using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Trigger : EventTrigger
{
    public UnityAction<bool> actionOnPointDown;
    public UnityAction<bool> actionOnPointUp;

    public override void OnPointerDown(PointerEventData data)
    {
        actionOnPointDown(true);
    }


    public override void OnPointerUp(PointerEventData data)
    {
        actionOnPointUp(false);
    }

}