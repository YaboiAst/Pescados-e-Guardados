using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;

    private void Update()
    {
        // Move forward and backward
        float moveInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * (moveInput * _moveSpeed * Time.deltaTime));

        // Rotate left and right
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, rotationInput * _rotationSpeed * Time.deltaTime);
    }
}