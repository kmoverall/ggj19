using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaitSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var customer = eventData.pointerDrag.GetComponent<Customer>();
        if (customer != null)
        {
            customer.WaitingOnShip = false;
        }
    }
}
