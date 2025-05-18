using UnityEngine;

public class Laser : MonoBehaviour
{
  [SerializeField] private float spinSpeed;

  private void Update()
  {
    spin();
  }

  private void spin()
  {
    transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
  }
}
