using System;
using UnityEngine;

/// <summary>
/// Example abilities scripts
/// </summary>
public class TestPlayerAbilities : aAbilities
{
    /// <summary>
    /// NOTE THESE MAY BE MOVED TO aAbilities
    /// </summary>
    public string HumanActiveAbilityName;
    public string HumanActiveAbilityDescription;
    public float HumanActiveAbilityCooldown;
    public Sprite HumanActiveAbilityImage;
    private AbilityDetails HumanActiveAbility;

    public string HumanPassiveAbilityName;
    public string HumanPassiveAbilityDescription;
    public float HumanPassiveAbilityCooldown;
    public Sprite HumanPassiveAbilityImage;
    private AbilityDetails HumanPassiveAbility;

    public string WerebeastActiveAbilityName;
    public string WerebeastActiveAbilityDescription;
    public float WerebeastActiveAbilityCooldown;
    public Sprite WerebeastActiveAbilityImage;
    private AbilityDetails WerebeastActiveAbility;

    public string WerebeastPassiveAbilityName;
    public string WerebeastPassiveAbilityDescription;
    public float WerebeastPassiveAbilityCooldown;
    public Sprite WerebeastPassiveAbilityImage;
    private AbilityDetails WerebeastPassiveAbility;

    public void Start()
    {
        this.HumanActiveAbility = this.SetAbilityDetails(AbilityDetails.AbilityType.Active, AbilityDetails.PlayerForm.Human, this.HumanActiveAbilityImage, this.HumanActiveAbilityName, this.HumanActiveAbilityDescription, this.HumanActiveAbilityCooldown);
        this.HumanPassiveAbility = this.SetAbilityDetails(AbilityDetails.AbilityType.Passive, AbilityDetails.PlayerForm.Human, this.HumanPassiveAbilityImage, this.HumanPassiveAbilityName, this.HumanPassiveAbilityDescription, this.HumanPassiveAbilityCooldown);
        this.WerebeastActiveAbility = this.SetAbilityDetails(AbilityDetails.AbilityType.Active, AbilityDetails.PlayerForm.Werebeast, this.WerebeastActiveAbilityImage, this.WerebeastActiveAbilityName, this.WerebeastActiveAbilityDescription, this.WerebeastActiveAbilityCooldown);
        this.WerebeastPassiveAbility = this.SetAbilityDetails(AbilityDetails.AbilityType.Passive, AbilityDetails.PlayerForm.Werebeast, this.WerebeastPassiveAbilityImage, this.WerebeastPassiveAbilityName, this.WerebeastPassiveAbilityDescription, this.WerebeastPassiveAbilityCooldown);
    }

    public override void UseActiveAbility()
    {
        if (!this.isWerebeast)
        {
            // Use human ability (increase move speed by 20%)
            this.ApplyMovementChange(this.HumanActiveAbility.abilityCooldown, 20);
        }
        else
        {
            // Use werebeast ability (increase move speed by 50%)
            this.ApplyMovementChange(this.WerebeastActiveAbility.abilityCooldown, 50);
        }
    }

    public override void UsePassiveAbiliy()
    {
        throw new NotImplementedException();
    }

    public override void UseTeamAbility()
    {
        throw new NotImplementedException();
    }
}
