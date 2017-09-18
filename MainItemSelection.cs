using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainItemSelection : MonoBehaviour
{
    /// <summary>
    /// The two selected items from index 0 to 3
    /// </summary>
    public int selectedItem1 = 0;
    public int selectedItem2 = 1;

    /// <summary>
    /// Colors for the two arrows (can be changed in the inspector)
    /// </summary>
    public Color arrowColor1;
    public Color arrowColor2;

    /// <summary>
    /// Array of arrow images, use 0-left, 1-up, 2-right, 3-down
    /// </summary>
    public Image[] arrows = new Image[4];

    /// <summary>
    /// Array of images used for item slots, use 0-left, 1-up, 2-right, 3-down (matches with arrows)
    /// </summary>
    public Image[] slotImage = new Image[4];

    /// <summary>
    /// Array of gameobjects to be stored, use 0-left, 1-up, 2-right, 3-down (matches with arrows)
    /// </summary>
    //public GameObject[] inventoryItems = new GameObject[4];
    public List<item> inventoryItems = new List<item>();

    /// <summary>
    /// Indicates if we have paused long enough and can change
    /// </summary>
    private static bool pausedLongEnough = true;

    /// <summary>
    /// Indicates if the arrows can change
    /// </summary>
    private static bool allowedToChangeMainItem = true;

    /// <summary>
    /// To control the sensitivity when changing the item selected
    /// Current setting will force the arrow to wait 5 frames before changing again
    /// </summary>
    private int pauseBuffer = 0;
    private int pauseFrames = 5;

    public GameObject mainInventory;

	/// <summary>
    /// Method for initialization
    /// </summary>
	void Start ()
    {
        for (int i = 0; i < 4; i++)
        {
            inventoryItems.Add(new item());
        }
    }

    /// <summary>
    /// Update method is called once per frame
    /// </summary>
    void Update()
    {
        inventoryItems = mainInventory.GetComponent<inventory>().inventoryList;
        if (pausedLongEnough && allowedToChangeMainItem)
        {
            // Mouse scrollwheel registers as 0.1, 0.2 or 0.3 depending on the speed
            if (Input.GetAxis("Mouse ScrollWheel") >= 0.08f)
            {
                // Scroll up - rotate arrow clockwise
                // Not allowed to change item for now
                pausedLongEnough = false;
                if (Input.GetAxis("Secondary Item") == 1)
                {
                    // Secondary item key is pressed so arrow2 changes
                    // Increase the index
                    selectedItem2++;
                    // Reset the index to 0 once we hit a value greater than 3
                    if (selectedItem2 > 3)
                    {
                        selectedItem2 = 0;
                    }

                    // Check for if an arrow gets placed on top of another arrow
                    if (selectedItem1 == selectedItem2)
                    {
                        selectedItem2++;
                        if (selectedItem2 > 3)
                        {
                            selectedItem2 = 0;
                        }
                    }
                }
                else
                {
                    // Arrow1 changes
                    selectedItem1++;
                    if (selectedItem1 > 3)
                    {
                        selectedItem1 = 0;
                    }

                    if (selectedItem1 == selectedItem2)
                    {
                        selectedItem1++;
                        if (selectedItem1 > 3)
                        {
                            selectedItem1 = 0;
                        }
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") <= -0.08f)
            {
                // Scroll down - rotate arrow counterclockwise
                pausedLongEnough = false;
                if (Input.GetAxis("Secondary Item") == 1)
                {
                    selectedItem2--;
                    if (selectedItem2 < 0)
                    {
                        selectedItem2 = 3;
                    }

                    if (selectedItem1 == selectedItem2)
                    {
                        selectedItem2--;
                        if (selectedItem2 < 0)
                        {
                            selectedItem2 = 3;
                        }
                    }
                }
                else
                {
                    selectedItem1--;
                    if (selectedItem1 < 0)
                    {
                        selectedItem1 = 3;
                    }

                    if (selectedItem1 == selectedItem2)
                    {
                        selectedItem1--;
                        if (selectedItem1 < 0)
                        {
                            selectedItem1 = 3;
                        }
                    }
                }
            }

            // Enable arrows depending on selectedItem1 and selectedItem2
            for (int index = 0; index < 4; index++)
            {
                // Arrows enabled and set to respective colors, others disabled
                if (index == selectedItem1)
                {
                    arrows[index].color = arrowColor1;
                    arrows[index].enabled = true;
                }
                else if (index == selectedItem2)
                {
                    arrows[index].color = arrowColor2;
                    arrows[index].enabled = true;
                }
                else
                {
                    arrows[index].enabled = false;
                }
            }
        }
        else
        {
            // Utilize a counter to count frames
            this.pauseBuffer++;
            if (this.pauseBuffer >= this.pauseFrames)
            {
                // Reset counter and allow for item change
                this.pauseBuffer = 0;
                pausedLongEnough = true;
            }
        }

        // Update the slot images based on what is in the inventory
        for (int index = 0; index < 4; index++)
        {
            if (inventoryItems[index] != new item())
            {
                slotImage[index].sprite = inventoryItems[index].icon.sprite;
                slotImage[index].enabled = true;
            }
            else
            {
                slotImage[index].enabled = false;
            }
        }
    }

    public static void SwitchMainItemSelection(bool OnOff)
    {
        pausedLongEnough = OnOff;
        allowedToChangeMainItem = OnOff;
    }
}