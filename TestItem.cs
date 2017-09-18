using System;
using UnityEngine;

public class TestItem : MonoBehaviour, InventoryItem
{
    public string name;
    public string description;
    public Sprite icon;

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetName()
    {
        return name;
    }
}
