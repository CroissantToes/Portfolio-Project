using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int orderNumber;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HUDManager.Instance.ShowDescriptionBox(orderNumber);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HUDManager.Instance.HideDescriptionBox();
    }
}
