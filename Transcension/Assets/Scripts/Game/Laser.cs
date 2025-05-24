using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float spinSpeed;

    private void Update()
    {
      spin();
    }

    /// <summary>
    /// Spins the object in the z axis at a set spinSpeed.
    /// </summary>
    private void spin()
    {
      transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
