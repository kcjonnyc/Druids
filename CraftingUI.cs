using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{

    /// <summary>
    /// Crafting wheel game objects, serves as container and background
    /// </summary>
    public GameObject leftCircle;
    public GameObject rightCircle;

    /// <summary>
    /// Craft resultant item box
    /// </summary>
    public GameObject craftResultBox;

    /// <summary>
    /// Slot container object, main object to rotate
    /// </summary>
    public GameObject leftSlotContainer;
    public GameObject rightSlotContainer;

    public GameObject leftInventory;
    public GameObject rightInventory;
    /// <summary>
    /// Item slots contains slot background, child contains item image
    /// </summary>
    public Image[] leftItemSlots = new Image[8];
    public Image[] rightItemSlots = new Image[8];

    /// <summary>
    /// Physical gameobjects to be placed in the inventory
    /// </summary>
    public List<item> leftInventoryItems = new List<item>();
    public List<item> rightInventoryItems = new List<item>();

    /// <summary>
    /// Main rotation speed
    /// </summary>
    public float rotateSpeed = 3.7f;

    /// <summary>
    /// Value to use delta time speed compensation
    /// </summary>
    public float deltaTimeSpeedAdjustment = 60;

    /// <summary>
    /// Angle which a slot should ignore mouse and try to stop
    /// </summary>
    public float angleStopGap = 5;

    /// <summary>
    /// Boolean indicating if we should invert crafting wheel controls
    /// </summary>
    public bool invertCraftingWheelRotation = false;

    /// <summary>
    /// Boolean indicating if we should show the inventory
    /// </summary>
    public bool showInventory;

    /// <summary>
    /// Transform object for the left ring and right ring
    /// </summary>
    private RectTransform leftRingTransform;
    private RectTransform rightRingTransform;

    /// <summary>
    /// Previous angle of rings for box adjustment
    /// </summary>
    private float leftRingPreviousAngle = 0;
    private float rightRingPreviousAngle = 0;

    /// <summary>
    /// Current frame angle to rotate
    /// </summary>
    private float leftAngleToRotate = 0;
    private float rightAngleToRotate = 0;

    /// <summary>
    /// Indicates the input on each scroll wheel
    /// </summary>
    private float leftMouseScrollWheel = 0;
    private float rightMouseScrollWheel = 0;

    /// <summary>
    /// Boolean values indicate if it has been the first rotation after
    /// being locked in a slot
    /// </summary>
    private bool leftInitialRotate = true;
    private bool rightInitialRotate = true;

    /// <summary>
    /// Method called on start
    /// </summary>
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            leftInventoryItems.Add(new item());
            rightInventoryItems.Add(new item());
        }
        // All crafting initially hidden
        this.leftCircle.SetActive(showInventory);
        this.rightCircle.SetActive(showInventory);
        this.craftResultBox.SetActive(showInventory);

        // Get transform components on start
        leftRingTransform = this.leftSlotContainer.GetComponent<RectTransform>();
        rightRingTransform = this.rightSlotContainer.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    void Update()
    {
        leftInventoryItems = leftInventory.GetComponent<inventory>().inventoryList;
        rightInventoryItems = rightInventory.GetComponent<inventory>().inventoryList;
        if (Input.GetButtonUp("Show Inventory"))
        {
            // Toggle inventory display
            showInventory = !showInventory;
            // Turn off crosshair when crafting menu is shown
            Crosshair.DisplayCrosshair(!showInventory);
            // Turn off main item selection control when crafting menu is shown
            MainItemSelection.SwitchMainItemSelection(!showInventory);
        }

        this.leftCircle.SetActive(showInventory);
        this.rightCircle.SetActive(showInventory);
        this.craftResultBox.SetActive(showInventory);

        if (showInventory)
        {
            // Display items in slots
            // NOTE* this could be changed to only grabbed the images on initialize and add
            // Left items used for crafting materials
            for (int index = 0; index < leftInventoryItems.Count; index++)
            {
                Image imageComponent = leftItemSlots[index].transform.GetChild(0).GetComponent<Image>();
                if (leftInventoryItems[index] != new item())
                {
                    item selectedItem = leftInventoryItems[index];
                    if (selectedItem != new item())
                    {
                        imageComponent.sprite = selectedItem.icon.sprite;
                        imageComponent.enabled = true;
                    }
                    else
                    {
                        imageComponent.enabled = false;
                    }
                }
                else
                {
                    imageComponent.enabled = false;
                }
            }

            // Left items used for primative materials
            for (int index = 0; index < rightInventoryItems.Count; index++)
            {
                Image imageComponent = rightItemSlots[index].transform.GetChild(0).GetComponent<Image>();
                if (rightInventoryItems[index] != new item())
                {
                    item selectedItem = rightInventoryItems[index];
                    if (selectedItem != new item())
                    {
                        imageComponent.sprite = selectedItem.icon.sprite;
                        imageComponent.enabled = true;
                    }
                    else
                    {
                        imageComponent.enabled = false;
                    }
                }
                else
                {
                    imageComponent.enabled = false;
                }
            }

            // Rotate slots based on the previous angle and current angle (to maintain orientation)
            float leftRingCurrentAngle = leftRingTransform.rotation.eulerAngles.z;
            foreach (Image itemSlot in leftItemSlots)
            {
                itemSlot.rectTransform.Rotate(Vector3.forward, leftRingPreviousAngle - leftRingCurrentAngle);
            }

            leftRingPreviousAngle = leftRingCurrentAngle;

            float rightRingCurrentAngle = rightRingTransform.rotation.eulerAngles.z;
            foreach (Image itemSlot in rightItemSlots)
            {
                itemSlot.rectTransform.Rotate(Vector3.forward, rightRingPreviousAngle - rightRingCurrentAngle);
            }

            rightRingPreviousAngle = rightRingCurrentAngle;

            // Mouse wheel control
            if (Input.GetAxis("Secondary Item") == 1)
            {
                // Rotate left wheel
                leftMouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
                rightMouseScrollWheel = 0;
            }
            else
            {
                // Rotate right wheel
                rightMouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
                leftMouseScrollWheel = 0;
            }

            // Right scroll wheel rotation
            if (rightMouseScrollWheel >= 0.1f && rightMouseScrollWheel != 0)
            {
                if (this.rightInitialRotate == true)
                {
                    // Initially, we want to rotate a value greater than the stopGap so it does not get stuck
                    this.rightAngleToRotate = this.angleStopGap + 0.01f;
                    this.rightInitialRotate = false;
                }
                else
                {
                    this.rightAngleToRotate = this.rotateSpeed + this.deltaTimeSpeedAdjustment * Time.deltaTime;
                }
            }
            else if (rightMouseScrollWheel <= -0.1f && rightMouseScrollWheel != 0)
            {
                if (this.rightInitialRotate == true)
                {
                    this.rightAngleToRotate = -1 * (this.angleStopGap + 0.01f);
                    this.rightInitialRotate = false;
                }
                else
                {
                    this.rightAngleToRotate = -this.rotateSpeed + -this.deltaTimeSpeedAdjustment * Time.deltaTime;
                }
            }

            // Left scroll wheel rotation
            if (leftMouseScrollWheel >= 0.1f && leftMouseScrollWheel != 0)
            {
                if (this.leftInitialRotate == true)
                {
                    this.leftAngleToRotate = this.angleStopGap + 0.01f;
                    this.leftInitialRotate = false;
                }
                else
                {
                    this.leftAngleToRotate = this.rotateSpeed + this.deltaTimeSpeedAdjustment * Time.deltaTime;
                }
            }
            else if (leftMouseScrollWheel <= -0.1f && leftMouseScrollWheel != 0)
            {
                if (this.leftInitialRotate == true)
                {
                    this.leftAngleToRotate = -1 * (this.angleStopGap + 0.01f);
                    this.leftInitialRotate = false;
                }
                else
                {
                    this.leftAngleToRotate = -this.rotateSpeed + -this.deltaTimeSpeedAdjustment * Time.deltaTime;
                }
            }
        }

        // Right scroll wheel stop and physical rotation
        // NOTE* The stop operations should still occur when the wheels are not shown
        if (Math.Abs(rightRingTransform.rotation.eulerAngles.z % 45) < this.angleStopGap && rightMouseScrollWheel == 0)
        {
            this.rightAngleToRotate = -1 * rightRingTransform.rotation.eulerAngles.z % 45;
            this.rightRingTransform.Rotate(Vector3.forward, rightAngleToRotate);
            this.rightAngleToRotate = 0;
            this.rightInitialRotate = true;
        }
        else
        {
            if (this.invertCraftingWheelRotation)
            {
                this.rightRingTransform.Rotate(Vector3.forward, -rightAngleToRotate);
            }
            else
            {
                this.rightRingTransform.Rotate(Vector3.forward, rightAngleToRotate);
            }
        }

        // Left scroll wheel stop
        if (Math.Abs(leftRingTransform.rotation.eulerAngles.z % 45) < this.angleStopGap && leftMouseScrollWheel == 0)
        {
            this.leftAngleToRotate = -1 * leftRingTransform.rotation.eulerAngles.z % 45;
            this.leftRingTransform.Rotate(Vector3.forward, leftAngleToRotate);
            this.leftAngleToRotate = 0;
            this.leftInitialRotate = true;
        }
        else
        {
            if (this.invertCraftingWheelRotation)
            {
                this.leftRingTransform.Rotate(Vector3.forward, -leftAngleToRotate);
            }
            else
            {
                this.leftRingTransform.Rotate(Vector3.forward, leftAngleToRotate);
            }
        }
    }

    /// <summary>
    /// Gets left slot number
    /// </summary>
    /// <returns>Left slot number</returns>
    public int GetLeftSlotNumber()
    {
        return ((int)(leftRingTransform.rotation.eulerAngles.z / 45) + 2) % 8;
    }

    /// <summary>
    /// Gets right slot number
    /// </summary>
    /// <returns>Right slot number</returns>
    public int GetRightSlotNumber()
    {
        return ((int)(rightRingTransform.rotation.eulerAngles.z / 45) + 6) % 8;
    }

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">Item to be added to the inventory</param>
    /// <returns>Returns a value indicating if the item was successfully added</returns>
    public bool AddItemToInventory(item selectedItem)
    {
        if (selectedItem == null)
        {
            // No item passed
            return false;
        }
        else
        {
            if (!selectedItem.material && !selectedItem.primitive)
            {
                // Items do not have required commonents and cannot be added to the inventory
                return false;
            }
            else if (selectedItem.material)
            {
                // Add to the craft materials inventory
                return AddToLeftSlot(selectedItem);
            }
            else if (selectedItem.primitive)
            {
                // Add to the primative materials inventory
                return AddToRightSlot(selectedItem);
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Adds item to the left slot
    /// </summary>
    /// <param name="item">Item to be added</param>
    /// <returns>Returns a value indicating if the item was successfully added</returns>
    private bool AddToLeftSlot(item selectedItem)
    {
        for (int index = 0; index < leftInventoryItems.Count; index++)
        {
            if (leftInventoryItems[index] == null)
            {
                leftInventoryItems[index] = selectedItem;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Adds item to the right slot
    /// </summary>
    /// <param name="item">Item to be added</param>
    /// <returns>Returns a value indicating if the item was successfully added</returns>
    private bool AddToRightSlot(item selectedItem)
    {
        for (int index = 0; index < rightInventoryItems.Count; index++)
        {
            if (rightInventoryItems[index] == null)
            {
                rightInventoryItems[index] = selectedItem;
                return true;
            }
        }
        return false;
    }
}
