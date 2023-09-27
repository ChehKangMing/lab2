using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player; // mario's transform
    public Transform endLimit; // GameObject that indicates end of map
    private float offset; // initial x-offset between camera and mario
    private float startx; // smallest x-coordinate of the camera
    private float endx; // largest x-coordinate of the camera
    private float viewportHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        // get coordinate of the bottom left of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);
        offset = this.transform.position.x - player.position.x;
        startx = this.transform.position.x;
        endx = endLimit.transform.position.x - viewportHalfWidth;
    }

    // Update is called once per frame
    void Update()
    {
        float desiredX = player.position.x + offset;
        // check if desired x is within start x and end x
        if (desiredX > startx && desiredX < endx)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }
}
