using System;
using System.Collections;
using UnityEngine;

/*TODO
 * Investigate immunity stacking (does it reset properly) - test the ApplyImmunity method before it finishes executing
 *      This one should work fine
 * 
 * Investigate movement speed debuff stacking
 *      If we are only working with percentages this will work fine - apply percentage at beginning and then divide out afterwards
 *      
 * Inventigate bleeding stacking
 *      Currently no stacking for bleeding (doesn't look like we need it from ability descriptions)
 *      Need to consider finding a way to restart the bleeding when there is already bleeding applied
 * */

abstract public class aAbilities : MonoBehaviour, IAbilities
{
    /// <summary>
    /// Ability slots that will need to be updated
    /// </summary>
    public ActiveAbilitySlot activeAbilitySlot;
    public BaseAbilitySlot passiveAbilitySlot;

    /// <summary>
    /// Movement script to be affected
    /// NOTE: This needs to be grabbed before movement can be affected
    /// If we need to change the enemy stats (add a debuff) enemy movement script needs to be grabbed
    /// </summary>
	public Movement movementScript;

    /// <summary>
    /// Percentage which is the lowest that a player's movement speed can be reduced to
    /// </summary>
    protected const float LOWESTMOVEMENTSPEEDPERCENT = 20f;

    /// <summary>
    /// Ability detail objects for character
    /// </summary>
    protected AbilityDetails humanActiveAbility;
    protected AbilityDetails humanPassiveAbility;
    protected AbilityDetails werebeastActiveAbility;
    protected AbilityDetails werebeastPassiveAbility;

    /// <summary>
    /// Stats script to be affected
    /// NOTE: This needs to be grabbed before stats can be affected
    /// If we need to change the enemy stats (add a debuff) enemy stats script needs to be grabbed
    /// </summary>
	protected Stats stats;

    /// <summary>
    /// Out player stats script, should be obtained on start
    /// </summary>
    protected Stats myPlayerStats;

    /// <summary>
    /// True if player is in human form, false if in werebeast form
    /// </summary>
    protected bool isWerebeast;

    /// <summary>
    /// Tracks the current movement speed percentage out of 100%
    /// </summary>
    private float currentMovementSpeedPercentage = 100f;

    /// <summary>
    /// Tracks the active ability cooldown remaining
    /// </summary>
    private float activeCooldownRemaining = 0f;

    /// <summary>
    /// Update called every frame
    /// </summary>
    public void Update()
    {
        // Try to use active ability
        if (Input.GetButtonDown("Active Ability"))
        {
            if (this.activeCooldownRemaining <= 0)
            {
                // Timer for ability cooldown is set to ability cooldown value
                this.activeCooldownRemaining = this.GetActiveAbility().abilityCooldown;
                this.UseActiveAbility();
            }
        }
        else if (Input.GetButtonUp("Change Form (Test)"))
        {
            this.isWerebeast = !this.isWerebeast;
            // Update ability slots and reset cooldowns
            activeAbilitySlot.UpdateCurrentAbility();
            passiveAbilitySlot.UpdateCurrentAbility();
            this.activeCooldownRemaining = 0;
        }

        // if the ability is still on cooldown, count down
        if (this.activeCooldownRemaining > 0)
        {
            this.activeCooldownRemaining -= Time.deltaTime;
            this.activeAbilitySlot.CooldownText.text = Math.Ceiling(this.activeCooldownRemaining).ToString();
        }

        if (this.activeCooldownRemaining <= 0)
        {
            // once the ability is no longer on cooldown, reset text
            this.activeCooldownRemaining = 0;
            this.activeAbilitySlot.CooldownText.text = "";
        }

        // Update fill for cooldown image
        this.activeAbilitySlot.CooldownImage.fillAmount = this.activeCooldownRemaining / this.GetActiveAbility().abilityCooldown;
    }

    /// <summary>
    /// Set the ability details for every form (human vs werebeast) and type (active vs passive)
    /// </summary>
    /// <param name="abilityType">Ability type</param>
    /// <param name="playerForm">Player form</param>
    /// <param name="abilityImage">Ability image for UI</param>
    /// <param name="abilityName">Ability name for UI</param>
    /// <param name="abilityDescription">Ability description for UI</param>
    /// <param name="abilityCooldown">Ability cooldown for UI</param>
    public AbilityDetails SetAbilityDetails(AbilityDetails.AbilityType abilityType, AbilityDetails.PlayerForm playerForm, Sprite abilityImage, 
        string abilityName, string abilityDescription, float abilityCooldown)
    {
        // Use switch to generate AbilityDetails object for each case
        switch (playerForm)
        {
            case AbilityDetails.PlayerForm.Human:
                switch (abilityType)
                {
                    case AbilityDetails.AbilityType.Active:
                        this.humanActiveAbility = new AbilityDetails(abilityType, playerForm, abilityImage, abilityName, abilityDescription, abilityCooldown);
                        return this.humanActiveAbility;
                    case AbilityDetails.AbilityType.Passive:
                        this.humanPassiveAbility = new AbilityDetails(abilityType, playerForm, abilityImage, abilityName, abilityDescription, abilityCooldown);
                        return this.humanPassiveAbility;
                }
                break;
            case AbilityDetails.PlayerForm.Werebeast:
                switch (abilityType)
                {
                    case AbilityDetails.AbilityType.Active:
                        this.werebeastActiveAbility = new AbilityDetails(abilityType, playerForm, abilityImage, abilityName, abilityDescription, abilityCooldown);
                        return this.werebeastActiveAbility;
                    case AbilityDetails.AbilityType.Passive:
                        this.werebeastPassiveAbility = new AbilityDetails(abilityType, playerForm, abilityImage, abilityName, abilityDescription, abilityCooldown);
                        return this.werebeastPassiveAbility;
                }
                break;
        }

        return null;
    }

    /// <summary>
    /// Called to set immunity or reset immunity if already applied
    /// </summary>
    /// <param name="immunityDuration">Duration in seconds</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
	public void ApplyImmunity (float immunityDuration)
    {
		if (!this.myPlayerStats.immune)
        {
            // Set immunity
            StartCoroutine(SetImmunity(immunityDuration));
		}
        else
        {
            // Restart immunity coroutine
            StopCoroutine("SetImmunity");
            StartCoroutine(SetImmunity(immunityDuration));
        }

	}

    /// <summary>
    /// Helper method used to set immunity for certain duration
    /// </summary>
    /// <param name="immunityDuration">Duration in seconds</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
    private IEnumerator SetImmunity (float immunityDuration)
    {
        // Make the player immune
        this.myPlayerStats.immune = true;
        // Reset after certain duration
        yield return new WaitForSeconds(immunityDuration);
        this.myPlayerStats.immune = false;
    }

    /// <summary>
    /// Applies bleeding
    /// </summary>
    /// <param name="bleedingDuration">Duration for bleeding</param>
    /// <param name="bleedingDamage">Bleeding damage per second, per stack</param>
    public void ApplyBleeding (float bleedingDuration, float bleedingDamage)
    {
        // Would need to have enemy stats script by this point
        if (!this.stats.bleeding)
        {
            StartCoroutine(SetBleeding(bleedingDuration, bleedingDamage));
        }
    }

    /// <summary>
    /// Helper method used apply bleeding for a certain duration
    /// </summary>
    /// <param name="bleedingDuration">Duration for bleeding</param>
    /// <param name="bleedingDamage">Bleeding damage per second, per stack</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
	private IEnumerator SetBleeding (float bleedingDuration, float bleedingDamage)
    {
        // Bleeding damnage is applied to stats health per second
        this.stats.healthPerSecond -= bleedingDamage;
		yield return new WaitForSeconds (bleedingDuration);
        this.stats.healthPerSecond += bleedingDamage;
	}

    /// <summary>
    /// Reduces the speed of player
    /// </summary>
    /// <param name="duration">Time the movement speed change lasts for</param>
    /// <param name="percentSpeedChange">The speed change (+ve for percent increase, -ve for percent decrease</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
	public void ApplyMovementChange(float duration, float percentSpeedChange)
    {
        // Would need to have enemy movement script by this point

        // Calculate the percentage to be changed
        float percentSpeedChangeCalculated = percentSpeedChange;
        // Reminder, we use + as percentSpeedChange is -ve when we want a movement speed decrease
        if (this.currentMovementSpeedPercentage + percentSpeedChange < LOWESTMOVEMENTSPEEDPERCENT)
        {
            percentSpeedChangeCalculated = LOWESTMOVEMENTSPEEDPERCENT - this.currentMovementSpeedPercentage;
        }

        // Apply movement speed debuff
        StartCoroutine(SetMovementChange(duration, percentSpeedChangeCalculated));
    }

    /// <summary>
    /// Reduces the speed of player
    /// </summary>
    /// <param name="duration">Time the movement speed change lasts for</param>
    /// <param name="percentSpeedChange">The speed change (+ve for percent increase, -ve for percent decrease</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
	private IEnumerator SetMovementChange (float duration, float percentSpeedChange)
    {
        // Calculate speed to be changed using max movement speed and given percentage
        // We assume we are passed a valid percentage here (0% - 100%)
        float amountToChangeX = this.movementScript.GetMaxMoveSpeedX() * percentSpeedChange / 100;
        float amountToChangeZ = this.movementScript.GetMaxMoveSpeedZ() * percentSpeedChange / 100;
        this.currentMovementSpeedPercentage += percentSpeedChange;
        this.movementScript.ChangeMovementSpeedX(amountToChangeX);
        this.movementScript.ChangeMovementSpeedZ(amountToChangeZ);

        // Wait for debuff duration
        yield return new WaitForSeconds(duration);

        // Revert the movement speed change
        this.currentMovementSpeedPercentage -= percentSpeedChange;
        this.movementScript.ChangeMovementSpeedX(-amountToChangeX);
        this.movementScript.ChangeMovementSpeedZ(-amountToChangeZ);
    }

    /// <summary>
    /// Gets active ability
    /// </summary>
    /// <returns>AbilityDetails object</returns>
    public AbilityDetails GetActiveAbility()
    {
        if (!this.isWerebeast)
        {
            return this.humanActiveAbility;
        }
        else
        {
            return this.werebeastActiveAbility;
        }
    }

    /// <summary>
    /// Get passive ability
    /// </summary>
    /// <returns>AbilityDetails object</returns>
    public AbilityDetails GetPassiveAbility()
    {
        if (!this.isWerebeast)
        {
            return this.humanPassiveAbility;
        }
        else
        {
            return this.werebeastPassiveAbility;
        }
    }

    /// <summary>
    /// Uses current active ability
    /// </summary>
	public abstract void UseActiveAbility();

    /// <summary>
    /// Uses current passive ability
    /// </summary>
	public abstract void UsePassiveAbiliy();

    /// <summary>
    /// Uses team ability
    /// </summary>
	public abstract void UseTeamAbility();
}
