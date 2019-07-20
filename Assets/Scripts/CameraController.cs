using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float hSpd;
    float vSpd;
    Vector2 spd;
    float zoom = 35f;
    float zoomSensitivity = 2f;
    private Camera camera;

    void Start() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        hSpd = Input.GetAxisRaw("Horizontal");
        vSpd = Input.GetAxisRaw("Vertical");
        spd = new Vector2(hSpd, vSpd).normalized;

        if(Input.mouseScrollDelta.y < 0) {
            zoom += zoomSensitivity;
        } else if(Input.mouseScrollDelta.y > 0) {
            zoom -= zoomSensitivity;
        }
    }
    void LateUpdate()
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoom, 0.4f);
        transform.Translate(spd.x, spd.y, 0);
    }
}
