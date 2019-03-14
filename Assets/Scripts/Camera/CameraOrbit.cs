using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{// Distance the Camera is form world zero
    public float distance = 10f;

    public float xSpeed = 120f;
    public float ySpeed = 120f;

    public float yMin = 15f;
    public float yMax = 80f;

    private float x = 0.0f;
    private float y = 0.0f;
    // Use this for initialization
    void Start()
    {
        Vector3 euler = transform.eulerAngles;
        x = euler.y;
        y = euler.x;
    }
  
    
    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("i press buton");
            //hide the cursor
            Cursor.visible = false;
            // get input x and y

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            x += mouseX * xSpeed * Time.deltaTime;
            y -= mouseY * ySpeed * Time.deltaTime;

            y = Mathf.Clamp(y, yMin, yMax);
        }
        else
        {
            Cursor.visible = true;
        }
        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = -transform.forward * distance;


    }



}
