using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform cameraPoint;
    [SerializeField] float cameraSpeed;

    float x; float z;

    Vector3 direction;

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        direction = (Vector3.forward * moveVertical) + (Vector3.right * moveHorizontal);
        direction = direction.normalized * cameraSpeed * Time.deltaTime;
        cameraPoint.Translate(direction);
    }
}
