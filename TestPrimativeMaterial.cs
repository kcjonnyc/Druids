using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrimativeMaterial : MonoBehaviour, PrimativeMaterial
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
