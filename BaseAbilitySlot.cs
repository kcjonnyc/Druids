using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseAbilitySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public aAbilities Abilities;
    public AbilityDetails.AbilityType AbilityType;

    public GameObject ToolTip;
    public Text ToolTipAbilityName;
    public Text ToolTipAbilityCooldown;
    public Text ToolTipAbilityDescription;

    private Sprite AbilityImage;
    private string AbilityName;
    private string AbilityDescription;
    private float AbilityCooldown;

    // Use this for initialization
    void Start()
    {
        this.UpdateCurrentAbility();
        this.ToolTip.SetActive(false);
    }

    public void UpdateCurrentAbility()
    {
        AbilityDetails abilityDetails;
        if (this.AbilityType == AbilityDetails.AbilityType.Active)
        {
            abilityDetails = this.Abilities.GetActiveAbility();
        }
        else
        {
            abilityDetails = this.Abilities.GetPassiveAbility();
        }

        this.AbilityImage = abilityDetails.abilityImage;
        this.AbilityName = abilityDetails.abilityName;
        this.AbilityDescription = abilityDetails.abilityDescription;
        this.AbilityCooldown = abilityDetails.abilityCooldown;

        // Update tooltip
        this.ToolTipAbilityName.text = this.AbilityName;
        this.ToolTipAbilityCooldown.text = "Cooldown: " + this.AbilityCooldown + "s";
        this.ToolTipAbilityDescription.text = this.AbilityDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Cursor.visible)
        {
            this.ToolTip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.ToolTip.SetActive(false);
    }
}
