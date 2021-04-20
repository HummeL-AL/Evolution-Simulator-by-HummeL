using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float sensivity;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize -= cam.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButton(2))
        {
            cam.transform.position -= sensivity * (cam.orthographicSize * new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f));
        }
    }
}
