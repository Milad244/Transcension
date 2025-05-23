using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;
    private float posXMaxLim;
    private float posXMinLim;
    private float floorLimit;
    private float cameraPosX;
    private float cameraPosY;

    private void Update()
    {
        cameraPosX = player.position.x;

        if (cameraPosX > posXMaxLim)
            cameraPosX = posXMaxLim;
        else if (cameraPosX < posXMinLim)
            cameraPosX = posXMinLim;

        
        if (player.position.y < floorLimit + 2) 
            cameraPosY = floorLimit;
        else
            cameraPosY = player.position.y - 2;

        transform.position = new Vector3(cameraPosX, cameraPosY, transform.position.z);
    }

    public void changeFloorLimit(float floorLimit)
    {
        this.floorLimit = floorLimit;
    }

    public void changeWallLimit(float posXMinLim_, float posXMaxLim_)
    {
        //Debug.Log(mainCamera.orthographicSize); //6
        //Debug.Log(mainCamera.aspect); //1.77836
        // Get the width of the camera's orthographic view
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        // Adjust the limits so the camera's edges don't pass the wall boundaries
        posXMinLim = posXMinLim_ + cameraHalfWidth; // Left edge of the camera
        posXMaxLim = posXMaxLim_ - cameraHalfWidth; // Right edge of the camera
    }
}
