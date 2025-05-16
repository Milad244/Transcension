using UnityEngine;

public class Laser : MonoBehaviour
{
  [SerializeField] private GameObject laserBeamPrefab;
  [SerializeField] private float spinSpeed;
  [SerializeField] private float laserLength;

  private void Awake()
  {
    spawnLasers();
  }

  private void spawnLasers()
  {
    Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
    };

    foreach (Vector3 dir in directions)
    {
      GameObject laser = Instantiate(laserBeamPrefab, transform);
      laser.transform.localPosition = dir * (laserLength / 2f);
      laser.transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);
      laser.transform.localScale = new Vector3(laserLength, laser.transform.localScale.y, laser.transform.localScale.z);
    }
  }

  private void Update()
  {
    spin();
  }

  private void spin()
  {
    transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
  }
}
