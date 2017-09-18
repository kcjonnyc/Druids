using UnityEngine;
using UnityEngine.UI;

public class HealthBarTimer : MonoBehaviour
{
    Image imageToFill;
    float totalTime = 10;
    float timeRemaining;

	// Initialization
    void Start()
    {
        imageToFill = this.GetComponent<Image>();
        timeRemaining = totalTime;
    }

    // Update is called once per frame
	// Change the fill of the timer object
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            imageToFill.fillAmount = timeRemaining / totalTime;
        }
    }
}