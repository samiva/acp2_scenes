using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{

    public float movementSpeed;
    public float rotationSpeed;

    public bool canMove;

    private float mPitch, mYaw;
    private CharacterController mController;

    // Start is called before the first frame update
    void Start()
    {
        mPitch = mYaw = 0;
        mController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        mPitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        mPitch = Mathf.Clamp(mPitch, -90, 90);
        mYaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        mYaw = mYaw > 360f ? 0 : mYaw;
        mYaw = mYaw < 0 ? 360 : mYaw;

        transform.rotation = Quaternion.Euler(mPitch, mYaw, 0);
        if (canMove)
        {
            var forwardMovement = transform.forward * Input.GetAxis("Vertical");
            var sideMovement = transform.right * Input.GetAxis("Horizontal");
            var dir = (forwardMovement + sideMovement).normalized;
            mController.Move(dir * movementSpeed * Time.deltaTime);
        }



    }
}
