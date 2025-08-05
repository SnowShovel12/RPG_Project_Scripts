using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static InventorySlot slotHoveredOver;
    public static GameObject tempImageBeingDragged;
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventorySO inventoryObject;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs();

        inventoryObject.InitializeSlots();

        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger component found!");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = slotUIs[go];
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnStartDrag(GameObject go)
    {
        MouseData.tempImageBeingDragged = CreateDragImage(go);
    }

    private GameObject CreateDragImage(GameObject go)
    {
        if (slotUIs[go].item.id < 0)
        {
            return null;
        }

        GameObject dragImageGo = new GameObject();
        RectTransform rectTransform = dragImageGo.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImageGo.transform.SetParent(transform.parent);

        Image image = dragImageGo.AddComponent<Image>();
        image.sprite = slotUIs[go].ItemObject.icon;
        image.raycastTarget = false;

        dragImageGo.name = "Drag Image";

        return dragImageGo;
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.tempImageBeingDragged == null)
        {
            return;
        }

        MouseData.tempImageBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempImageBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotUIs[go].RemoveItem();
        }
        else if (MouseData.slotHoveredOver != null)
        {
            InventorySlot mouseHoverSlotData = MouseData.slotHoveredOver;
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slotUIs[go];
        if (slot == null)
        {
            return;
        }

        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }

        if (data.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(slot);
        }
    }

    public virtual void OnRightClick(InventorySlot slot)
    {

    }

    public virtual void OnLeftClick(InventorySlot slot)
    {

    }
}

