using UnityEngine;

public interface CraftingMaterial
{
    Sprite GetIcon();
    string GetName();
    string GetDescription();
}