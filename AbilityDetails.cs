using UnityEngine;

public class AbilityDetails
{
    public enum AbilityType { Active, Passive };
    public enum PlayerForm { Human, Werebeast };

    /// <summary>
    /// The ability type, either active or passive
    /// </summary>
    public AbilityType abilityType;

    /// <summary>
    /// The player form, either human or werebeast
    /// </summary>
    public PlayerForm playerForm;

    /// <summary>
    /// The sprite used for the ability image
    /// </summary>
    public Sprite abilityImage;

    /// <summary>
    /// Ability name
    /// </summary>
    public string abilityName;

    /// <summary>
    /// Ability description
    /// </summary>
    public string abilityDescription;

    /// <summary>
    /// Ability cooldown
    /// </summary>
    public float abilityCooldown;

    public AbilityDetails(AbilityDetails.AbilityType abilityType, AbilityDetails.PlayerForm playerForm,
        Sprite abilityImage, string abilityName, string abilityDescription, float abilityCooldown)
    {
        this.abilityType = abilityType;
        this.playerForm = playerForm;
        this.abilityImage = abilityImage;
        this.abilityName = abilityName;
        this.abilityDescription = abilityDescription;
        this.abilityCooldown = abilityCooldown;
    }
}
