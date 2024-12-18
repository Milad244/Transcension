using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float posXMaxLim;
    [SerializeField] private float posXMinLim;
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

        
        if (player.position.y < 3 + 2) 
            cameraPosY = 3;
        else
            cameraPosY = player.position.y - 2;

        transform.position = new Vector3(cameraPosX, cameraPosY, transform.position.z);
    }

    
}
