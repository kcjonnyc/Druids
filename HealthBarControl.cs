using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour {

    #region Public Variables

    /// <summary>
    /// The circular health bar image that we need to fill
    /// </summary>
    public Image circularHealthBar;

    /// <summary>
    /// The fill value where the health bar appears full on screen
    /// </summary>
    public float fillAmountFull = 1;

    /// <summary>
    /// The fill value where the health bar appears empty on screen
    /// </summary>
    public float fillAmountEmpty = 0;

    /// <summary>
    /// The percentage of health remaining
    /// </summary>
    public float healthPercentage = 100;

    #endregion

    #region Instance Variables

    /// <summary>
    /// The difference between full and empty fill amounts
    /// </summary>
    private float fillDifference;

    #endregion

    /// <summary>
    /// Initialization method
    /// </summary>
    private void Start()
    {
        circularHealthBar = this.GetComponent<Image>();
        this.fillDifference = this.fillAmountFull - this.fillAmountEmpty;
    }
	
	/// <summary>
    /// Update method called once per frame
    /// </summary>
	private void Update()
    {
        //// The fill amount will be the fill value when the bar appears to be empty in addition 
        //// to the difference multiplied by percentage of health remaining
        circularHealthBar.fillAmount = this.fillAmountEmpty + ((this.healthPercentage / 100) * this.fillDifference);
    }

    #region Helper Methods

    public void SetPercentage(float percentage)
    {
        this.healthPercentage = percentage;
    }

    #endregion
}
