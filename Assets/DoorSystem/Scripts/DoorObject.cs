using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Dialogue;
using UnityEngine;

[System.Serializable]
public class DoorInfo
{
    public ItemID keyId;
    public int DialogueId;
    public bool isLocked;
    public bool isOpened;

    public bool CheckCanOpen()
    {
        return !isLocked || Array.Exists(Inventory.instance.slots,itemSlot => itemSlot.item.itemId == keyId);
    }
}

public class DoorObject : InteractableObject
{
    public DialogueController2 dialogueController2; 
    [SerializeField] DoorInfo doorInfo;
    [SerializeField] GameObject door;

    public void DialogueControll()
    {
        dialogueController2.SetDialogueFlag(doorInfo.DialogueId);
    }

    protected override void ActOnTrigger(Collider2D other)
    {
        if (doorInfo.CheckCanOpen())
        {
            DialogueControll();
            door.SetActive(!door.activeSelf);
        }
    }

    private void Start()
    {
        if(doorInfo.isLocked && doorInfo.keyId == ItemID.NONE) 
        {
            Debug.LogWarning("This Door is Locked, But Has not Key. Please Door");
        }
    }
}
