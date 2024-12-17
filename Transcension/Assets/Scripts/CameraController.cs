using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float posXMaxLim;
    [SerializeField] private float posXMinLim;
    private float cameraPosX;

    private void Update()
    {
        if (player.position.x > posXMaxLim)
            cameraPosX = posXMaxLim;
        else if (player.position.x < posXMinLim)
            cameraPosX = posXMinLim;
        else
            cameraPosX = player.position.x;

        transform.position = new Vector3(cameraPosX, player.position.y + 3, transform.position.z);
    }

    
}
