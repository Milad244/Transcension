using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float posXMaxLim;
    [SerializeField] private float posXMinLim;
    private float floorLimit;
    private float cameraPosX;
    private float cameraPosY;

    private void Update()
    {
        if (player.position.x > posXMaxLim)
            cameraPosX = posXMaxLim;
        else if (player.position.x < posXMinLim)
            cameraPosX = posXMinLim;
        else
            cameraPosX = player.position.x;

        
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

    public void changeWallLimit(float posXMinLim, float posXMaxLim)
    {
        this.posXMinLim = posXMinLim;
        this.posXMaxLim = posXMaxLim;
    }
}
