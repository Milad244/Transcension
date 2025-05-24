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
        // Adjusting the camera so it doesn't go past my X (wall) and Y (floor) limits.
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

    /// <summary>
    /// Changes the minimum Y (floor) the camera can go.
    /// </summary>
    /// <param name="floorLimit">The y position of the minimum limit the camera can go.</param>
    public void changeFloorLimit(float floorLimit)
    {
        this.floorLimit = floorLimit;
    }

    /// <summary>
    /// Changes the minimum and maximum X (wall) limits the camera can go based on screen size.
    /// </summary>
    /// <param name="posXMinLim_">The left boundary of the level.</param>
    /// <param name="posXMaxLim_">The right boundary of the level.</param>
    public void changeWallLimit(float posXMinLim_, float posXMaxLim_)
    {
        //Debug.Log(mainCamera.orthographicSize); //6
        //Debug.Log(mainCamera.aspect); //1.77836

        // Finds half the camera’s width based on zoom and screen size. This keeps camera movement working right on all screen sizes.
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        posXMinLim = posXMinLim_ + cameraHalfWidth;
        posXMaxLim = posXMaxLim_ - cameraHalfWidth;
    }
}
