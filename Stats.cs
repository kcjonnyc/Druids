using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    #region Public Variables

    /// <summary>
    /// Player Level Variables
    /// </summary>
    public int maxPlayerLevel = 18;
    public int currentPlayerLevel = 1;
    public float maxExperience = 100f;
    public float currentExperience = 0f;
    public float experiencePerSecond = 5f;
    public GameObject experienceRing;
    public Text playerLevelText;

    /// <summary>
    /// Health Variables
    /// </summary>
    public float maxHealth = 100f;
	public float currentHealth = 100f;
	public float healthPerSecond = 1f;
	public GameObject healthBar;

    /// <summary>
    /// Hunger Variables
    /// </summary>
    public float maxHunger = 100f;
    public float currentHunger = 100f;
    public float hungerPerSecond = -1f;
    public float hungerDecreaseHealth = -1f;
    public GameObject hungerBar;

    /// <summary>
    /// Thirst Variables
    /// </summary>
    public float maxThirst = 100f;
	public float currentThirst = 100f;
    public float thirstPerSecond = -1f;
    public float thirstDecreaseHealth = -1f;
    public GameObject thirstBar;

    /// <summary>
    /// Energy Variables
    /// </summary>
	public float maxEnergy = 100f;
	public float currentEnergy = 100f;
	public float energyPerSecond = 1f;
	public GameObject energyBar;

    /// <summary>
    /// Player immunity
    /// </summary>
    public bool immune;

    /// <summary>
    /// Player bleeding
    /// </summary>
    public bool bleeding;

    #endregion

    #region Private Variables

    /// <summary>
    /// "Health" bar controllers for each statistic
    /// </summary>
    private HealthBarControl experienceControl;
    private HealthBarControl healthBarControl;
    private HealthBarControl hungerBarControl;
    private HealthBarControl thirstBarControl;
    private HealthBarControl energyBarControl;

    #endregion

    /// <summary>
    /// Method called for initialization
    /// </summary>
    public void Start ()
    {
        // Current health, thirst, hunger and energy starts at the maximum value
		this.currentHealth = this.maxHealth;
        this.currentThirst = this.maxThirst;
        this.currentHunger = this.maxHunger;
        this.currentEnergy = this.maxEnergy;

        // Get controller from the respective "health" bars
        this.experienceControl = this.experienceRing.GetComponent<HealthBarControl>();
        this.healthBarControl = this.healthBar.GetComponent<HealthBarControl>();
        this.hungerBarControl = this.hungerBar.GetComponent<HealthBarControl>();
        this.thirstBarControl = this.thirstBar.GetComponent<HealthBarControl>();
        this.energyBarControl = this.energyBar.GetComponent<HealthBarControl>();

        InvokeRepeating("increaseExperienceRepeating", 1f, 1f);
        InvokeRepeating("changeThirstRepeating", 1f, 1f);
		InvokeRepeating("changeHungerRepeating", 1f, 1f);
		InvokeRepeating("changeHealthRepeating", 1f, 1f);
		InvokeRepeating("changeEnergyRepeating", 1f, 1f);
	}

    #region Player Level Methods

    /// <summary>
    /// Method will change the current experience given a value
    /// </summary>
    /// <param name="increaseDecreaseExperienceValue">The value to increase or decrease current experience by</param>
    public void changeExperience(float increaseDecreaseExperienceValue)
    {
		this.currentExperience += increaseDecreaseExperienceValue;
		if (this.currentExperience >= this.maxExperience)
        {
			// Level up
            if (this.currentPlayerLevel < this.maxPlayerLevel)
            {
                this.currentPlayerLevel++;
                this.playerLevelText.text = this.currentPlayerLevel.ToString();
                // The experience over the max carries over to the next level
                this.currentExperience -= this.maxExperience;
            }
            else
            {
                this.currentExperience = this.maxExperience;
            }
		}

        this.updateExperience((this.currentExperience / this.maxExperience) * 100);
    }

    /// <summary>
    /// Method that is invoked for repeating experience change (based on experience per second)
    /// </summary>
	private void increaseExperienceRepeating()
    {
        this.changeExperience(this.experiencePerSecond);
	}

    /// <summary>
    /// Method to update experience bar and perform any other nessesary actions
    /// </summary>
    /// <param name="experiencePercentage">The percentage of experience remaining (until levelup)</param>
	private void updateExperience(float experiencePercentage)
    {
        this.experienceControl.SetPercentage(experiencePercentage);
	}

    #endregion

    #region Health Methods

    /// <summary>
    /// Method will change the current health given a value
    /// </summary>
    /// <param name="increaseDecreaseHealthValue">The value to increase or decrease current health by</param>
    public void changeHealth(float increaseDecreaseHealthValue)
    {
		this.currentHealth += increaseDecreaseHealthValue;
		if (this.currentHealth > this.maxHealth)
        {
			this.currentHealth = this.maxHealth;
		}
        else if (this.currentHealth < 0)
        {
            this.currentHealth = 0;
        }

        this.updateHealth((this.currentHealth / this.maxHealth) * 100);
    }

    /// <summary>
    /// Method that is invoked for repeating health change (based on health per second)
    /// </summary>
	private void changeHealthRepeating()
    {
        this.changeHealth(this.healthPerSecond);
	}

    /// <summary>
    /// Method to update health bar and perform any other nessesary actions
    /// </summary>
    /// <param name="healthPercentage">The percentage of health remaining</param>
	private void updateHealth (float healthPercentage)
    {
        this.healthBarControl.SetPercentage(healthPercentage);
	}

	///<summary>
	///Method to get the current player's Thirst percentage
	///</summary>	
	public float getCurrentHealth(){


		return currentHealth;
	}
    #endregion

    #region Hunger Methods

    /// <summary>
    /// Method will change the current hunger given a value
    /// </summary>
    /// <param name="increaseDecreaseHungerValue">The value to increase or decrease current hunger by</param>
    public void changeHunger(float increaseDecreaseHungerValue)
    {
        this.currentHunger += increaseDecreaseHungerValue;
        if (this.currentHunger > this.maxHunger)
        {
            this.currentHunger = this.maxHunger;
        }
        else if (this.currentHunger < 0)
        {
            this.currentHunger = 0;
        }

        this.updateHunger((this.currentHunger / this.maxHunger) * 100);
    }

    /// <summary>
    /// Method that is invoked for repeating hunger change (based on hunger per second)
    /// </summary>
    private void changeHungerRepeating()
    {
        this.changeHunger(this.hungerPerSecond);
        if (this.currentHunger <= 0)
        {
            // When hunger is at 0, a player's health should decrease
            this.changeHealth(this.hungerDecreaseHealth);
        }
    }

    /// <summary>
    /// Method to update hunger bar and perform any other nessesary actions
    /// </summary>
    /// <param name="hungerPercentage">The percentage of hunger remaining</param>
	private void updateHunger(float hungerPercentage)
    {
        this.hungerBarControl.SetPercentage(hungerPercentage);
    }
	///<summary>
	///Method to get the current player's hunnger percentage
	///</summary>
	public float getCurrentHunger(){


		return currentHunger;
	}
    #endregion

    #region Thirst Methods

    /// <summary>
    /// Method will change the current thirst given a value
    /// </summary>
    /// <param name="increaseDecreaseThirstValue">The value to increase or decrease current thirst by</param>
    public void changeThirst(float increaseDecreaseThirstValue)
    {
		this.currentThirst += increaseDecreaseThirstValue;
		if (this.currentThirst > this.maxThirst)
        {
			this.currentThirst = this.maxThirst;
		}
        else if (this.currentThirst < 0)
        {
            this.currentThirst = 0;
        }

        this.updateThirst((this.currentThirst / this.maxThirst) * 100);
    }

    /// <summary>
    /// Method that is invoked for repeating thirst change (based on thirst per second)
    /// </summary>
	private void changeThirstRepeating()
    {
        this.changeThirst(this.thirstPerSecond);
		if (this.currentThirst <= 0) {
            // When hunger is at 0, a player's health should decrease
            this.changeHealth(this.thirstDecreaseHealth);
        }
    }

    /// <summary>
    /// Method to update hunger bar and perform any other nessesary actions
    /// </summary>
    /// <param name="thirstPercentage">The percentage of thirst remaining</param>
	private void updateThirst (float thirstPercentage)
    {
        this.thirstBarControl.SetPercentage(thirstPercentage);
	}

	///<summary>
	///Method to get the current player's Thirst percentage
	///</summary>
	public float getCurrentThirst(){


		return currentThirst;
	}
    #endregion

    #region Energy Methods

    /// <summary>
    /// Method will change the current energy given a value
    /// </summary>
    /// <param name="increaseDecreaseEnergyValue">The value to increase or decrease current energy by</param>
    public void changeEnergy(float increaseDecreaseEnergyValue)
    {
		this.currentEnergy += increaseDecreaseEnergyValue;
		if (this.currentEnergy > this.maxEnergy)
        {
			this.currentEnergy = this.maxEnergy;
		}
        else if (this.currentEnergy < 0)
        {
            this.currentEnergy = 0;
        }

        this.updateEnergy((this.currentEnergy / this.maxEnergy) * 100);
    }

    /// <summary>
    /// Method that is invoked for repeating energy change (based on energy per second)
    /// </summary>
	private void changeEnergyRepeating()
    {
        this.changeEnergy(this.energyPerSecond);
	}

    /// <summary>
    /// Method to update energy bar and perform any other nessesary actions
    /// </summary>
    /// <param name="thirstPercentage">The percentage of energy remaining</param>
	private void updateEnergy (float energyPercentage)
    {
        this.energyBarControl.SetPercentage(energyPercentage);
	}

	///<summary>
	///Method to get the current player's energy percentage
	///</summary>
	public float getCurrentEnergy(){


		return currentEnergy;
	}
    #endregion


}
