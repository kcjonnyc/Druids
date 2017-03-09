using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	// move speed
    public float moveSpeedX = 10.0f;
    public float moveSpeedZ = 10.0f;
	public float jumpSpeed = 10;
	public float gravity = 10;

	// movement required
    public float movementX;
    public float movementZ;

	// rotation speed
    public float rotationSpeed = 100;

	// movement (rotation) required
    public float rotationH;
    public float rotationV;

	// camera object
    public Camera firstPersonCamera;

    public float height;
    private Rigidbody rb; 

    // Use this for initialization
    void Start () {
		// gets rigidbody object and height of object
        rb = GetComponent<Rigidbody>();
        height = GetComponent<Collider>().bounds.extents.y;
    }
	
	// Update is called once per frame
	void Update () {
		// movement calculations based on change in time and move speed
        movementX = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedX;
        movementZ = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedZ;
        rotationH = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        rotationV = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;

		// translation of object
        transform.Translate(movementZ, 0, movementX);
        transform.Rotate(0, rotationH, 0);
        firstPersonCamera.transform.Rotate(-rotationV, 0, 0, Space.Self);

		// jump is allowed if object is on the ground
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(0, jumpSpeed, 0);
        }
        rb.AddForce(0, -gravity * Time.deltaTime, 0);
    }

	// method to check if object is on the ground
    bool IsGrounded() {
		// raycast is fired, if it hits the ground, we know object is grounded
        return Physics.Raycast(transform.position, -Vector3.up, height + 0.1f);
    }

}
