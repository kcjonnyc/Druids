using UnityEngine;

public interface IAbilities
{
    /// <summary>
    /// Set the ability details for every form (human vs werebeast) and type (active vs passive)
    /// </summary>
    /// <param name="abilityType">Ability type</param>
    /// <param name="playerForm">Player form</param>
    /// <param name="abilityImage">Ability image for UI</param>
    /// <param name="abilityName">Ability name for UI</param>
    /// <param name="abilityDescription">Ability description for UI</param>
    /// <param name="abilityCooldown">Ability cooldown for UI</param>
	AbilityDetails SetAbilityDetails (AbilityDetails.AbilityType abilityType, AbilityDetails.PlayerForm playerForm, 
        Sprite abilityImage, string abilityName, string abilityDescription, float abilityCooldown);

    /// <summary>
    /// Used to set immunity for certain duration
    /// </summary>
    /// <param name="immunityDuration">Duration in seconds</param>
	void ApplyImmunity (float immunityDuration);

    /// <summary>
    /// Applies bleeding
    /// </summary>
    /// <param name="bleedingDuration">Duration for bleeding</param>
    /// <param name="bleedingDamage">Bleeding damage per second, per stack</param>
	void ApplyBleeding (float bleedingDuration, float bleedingDamage);

    /// <summary>
    /// Reduces the speed of player
    /// </summary>
    /// <param name="duration">Time the movement speed change lasts for</param>
    /// <param name="percentSpeedChange">The speed change (+ve for percent increase, -ve for percent decrease</param>
    /// <returns>IEnumerator needed for Unity Corountine</returns>
	void ApplyMovementChange (float duration, float lostSpeedPercentage);

    /// <summary>
    /// Uses current active ability
    /// </summary>
	void UseActiveAbility();

    /// <summary>
    /// Uses current passive ability
    /// </summary>
	void UsePassiveAbiliy();

    /// <summary>
    /// Uses team ability
    /// </summary>
	void UseTeamAbility();
}
