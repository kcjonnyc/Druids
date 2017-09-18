using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerAbility : MonoBehaviour
{
    public enum AbilityType { Active, Passive };

    public abstract AbilityType GetAbilityType();
    public abstract Sprite GetAbilityImage();
    public abstract string GetAbilityName();
    public abstract string GetAbilityDescription();
    public abstract float GetAbilityCooldown();
}
