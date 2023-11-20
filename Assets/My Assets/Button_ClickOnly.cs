using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_ClickOnly : MonoBehaviour, IPointerClickHandler
{
    public event Action onClick;

    // Start is called before the first frame update
    private void Awake()
    {
        onClick += Test;
    }

    private void OnDestroy()
    {
        onClick += Test;
    }

    public virtual void OnPointerClick(PointerEventData eventData) 
    {
        onClick?.Invoke();
    }

    void Test()
    {
        Debug.Log("I was prepared");
    } 
}
