using UnityEngine;

/// <summary>
/// This script controls all character movement
/// </summary>
public class Movement : MonoBehaviour
{
    /// <summary>
    /// The max move speed in the X and Z components
    /// </summary>
    public float maxMoveSpeedX = 10.0f;
    public float maxMoveSpeedZ = 10.0f;

    /// <summary>
    /// Rotation speed (mouse)
    /// </summary>
    public float rotationSpeed = 100; 

    /// <summary>
    /// The first person camera object to be controlled
    /// </summary>
    public Camera firstPersonCamera; 

    /// <summary>
    /// The player jump speed
    /// </summary>
    public float jumpSpeed = 10; 

    /// <summary>
    /// The gravity on the player
    /// </summary>
    public float gravity = 10;

    /// <summary>
    /// The value to be set as the run multiplier
    /// </summary>
    public float runSetting = 1.4f;

    /// <summary>
    /// The current move speed in the X and Z components
    /// </summary>
    private float moveSpeedX;
    private float moveSpeedZ;

    /// <summary>
    /// The distance the player that needs to be translated in the X and Z components
    /// </summary>
    private float movementX;
    private float movementZ;

    /// <summary>
    /// The horizontal and vertical rotation required
    /// </summary>
    private float rotationH;
    private float rotationV;

    /// <summary>
    /// Multiplier that is applied, changed to a value greater than 1 when running
    /// </summary>
    private float runMultipler = 1;

    /// <summary>
    /// Height variable to track if the player is off the ground
    /// </summary>
    private float height;

    /// <summary>
    /// Indicates if the mouse is shown
    /// </summary>
    private bool mouseShown = false;

    /// <summary>
    /// Rigidbody of the gameobject that the script is attached to
    /// </summary>
    private Rigidbody rb; 

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Start ()
    {
        // Initialize instance variables
        this.moveSpeedX = this.maxMoveSpeedX;
        this.moveSpeedZ = this.maxMoveSpeedZ;
        this.rb = this.GetComponent<Rigidbody>();
        this.height = this.GetComponent<Collider>().bounds.extents.y; 
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        // Apply run multiplier when button is clicked
        if (Input.GetAxis("Run") > 0)
        {
            this.runMultipler = this.runSetting;
        }
        else
        {
            this.runMultipler = 1;
        }

        // Toggle cursor and block movement when button clicked
        if (Input.GetButtonDown("Toggle Mouse"))
        {
            this.mouseShown = !this.mouseShown;
            Cursor.visible = this.mouseShown;
        }

        // Calculate movement based on move speed and the change in time
        this.movementX = Input.GetAxis("Vertical") * Time.deltaTime * this.moveSpeedX * this.runMultipler;
        this.movementZ = Input.GetAxis("Horizontal") * Time.deltaTime * this.moveSpeedZ * this.runMultipler;

        // Calculate rotation based on rotation speed and change in time
        this.rotationH = Input.GetAxis("Mouse X") * Time.deltaTime * this.rotationSpeed;
        this.rotationV = Input.GetAxis("Mouse Y") * Time.deltaTime * this.rotationSpeed;

        // Access the transform of the gameobject the script is attached to
        // Use the calculated movement to translate gameobject
        this.transform.Translate(movementZ, 0, movementX); 
        
        // Only apply rotation if the cursor/mouse is not shown 
        if (!this.mouseShown)
        {
            // Use the calculated rotation to physically rotate the gameobject horizontally
            // Use the calculated rotation to rotate the camera vertically
            this.transform.Rotate(0, this.rotationH, 0);
            this.firstPersonCamera.transform.Rotate(-this.rotationV, 0, 0, Space.Self);
        }

        // If the jump button is pressed and the used is on the ground, we can jump again
        if (Input.GetButtonDown("Jump") && this.IsGrounded()) {
            // Add force to the rigidbody component
            this.rb.AddForce(0, this.jumpSpeed, 0); 
        }

        // gravity is used to add force in a downwards direction on the rigidbody component
        this.rb.AddForce(0, -this.gravity * Time.deltaTime, 0);
    }

    /// <summary>
    /// Method to check if the user is on the ground
    /// </summary>
    /// <returns>True if the user is on the ground</returns>
    private bool IsGrounded()
    {
        // if raycast hits the ground froun the position of the object, it is on the ground
        return Physics.Raycast(this.transform.position, -Vector3.up, this.height + 0.1f);
    }

    /// <summary>
    /// Gets current movement speed in the X component
    /// </summary>
    /// <returns>Current movement speed in X</returns>
	public float GetMoveSpeedX()
    {
		return this.moveSpeedX;
	}

    /// <summary>
    /// Gets current movement speed in the Z component
    /// </summary>
    /// <returns>Current movement speed in Z</returns>
	public float GetMoveSpeedZ()
    {
		return this.moveSpeedZ;
	}

    /// <summary>
    /// Gets max movement speed in the X component
    /// </summary>
    /// <returns>Max movement speed in X</returns>
    public float GetMaxMoveSpeedX()
    {
        return this.maxMoveSpeedX;
    }

    /// <summary>
    /// Gets max movement speed in the Z component
    /// </summary>
    /// <returns>Max movement speed in Z</returns>
    public float GetMaxMoveSpeedZ()
    {
        return this.maxMoveSpeedZ;
    }

    /// <summary>
    /// Changes movement speed in the X component
    /// </summary>
    /// <param name="amountToChange">Amount to be changed</param>
	public void ChangeMovementSpeedX(float amountToChange)
    {
        this.moveSpeedX += amountToChange;
    }

    /// <summary>
    /// Changes movement speed in the Z component
    /// </summary>
    /// <param name="amountToChange">Amount to be changed</param>
    public void ChangeMovementSpeedZ(float amountToChange)
    {
        this.moveSpeedZ += amountToChange;
    }

    /// <summary>
    /// Changes max movement speed in the X component
    /// </summary>
    /// <param name="amountToChange">Amount to be changed</param>
    public void ChangeMaxMovementSpeedX(float amountToChange)
    {
        this.maxMoveSpeedX += amountToChange;
        this.moveSpeedX += amountToChange;
    }

    /// <summary>
    /// Changes max movement speed in the Z component
    /// </summary>
    /// <param name="amountToChange">Amount to be changed</param>
    public void ChangeMaxMovementSpeedZ(float amountToChange)
    {
        this.maxMoveSpeedZ += amountToChange;
        this.moveSpeedZ += amountToChange;
    }
}
