using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float PlayerSpeed;
    public float LookSensitivity;

    private float modifiedPlayerSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            modifiedPlayerSpeed = PlayerSpeed * 1.6f;
        }
        else
        {
            modifiedPlayerSpeed = PlayerSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + transform.forward * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + transform.forward * -1 * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + transform.right * -1 * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + transform.right * Time.deltaTime * modifiedPlayerSpeed;
        }

        float rotateHorizontal = Input.GetAxis("Mouse X");
        transform.Rotate(transform.up * rotateHorizontal * LookSensitivity);

        float rotateVertical = Input.GetAxis("Mouse Y");
        var playerCamera = GameObject.Find("Main Camera");
        var gun = GameObject.Find("45Gun");
        Vector3 localX = transform.TransformDirection(Vector3.right);
        //playerCamera.transform.Rotate(transform.right * rotateVertical * LookSensitivity);
        //gun.transform.Rotate(transform.right * rotateVertical * LookSensitivity);

        playerCamera.transform.RotateAround(playerCamera.transform.position, localX, rotateVertical * LookSensitivity);
        gun.transform.RotateAround(gun.transform.position, localX, rotateVertical * LookSensitivity);
    }
}

