using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramnBehaviour : MonoBehaviour {

    private float speed = 4f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    // Update is called once per frame
    void Update () {

        float InputX = Input.GetAxisRaw("Horizontal");
        float InputZ = Input.GetAxisRaw("Vertical");

        transform.Translate(transform.right * InputX * speed * Time.deltaTime);
        transform.Translate(transform.forward * InputZ * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q)) transform.Translate(transform.up * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Translate(-transform.up * speed * Time.deltaTime);

        float MouseX = Mathf.Max(Input.GetAxis("Mouse X")/* + Input.GetAxisRaw("HorizontalRightStick")*/);
        float MouseY = -Mathf.Max(Input.GetAxis("Mouse Y")/* + Input.GetAxisRaw("VerticalRightStick")*/);

        rotY += MouseX * 175.0f * Time.deltaTime;
        rotX += MouseY * 175.0f * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

    }
}
