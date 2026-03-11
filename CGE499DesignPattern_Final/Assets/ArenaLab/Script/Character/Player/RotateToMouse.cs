using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [Header("Target Camera")]
    [SerializeField] private Camera cam;

    [Header("Rotation")]
    [SerializeField] private float rotationOffset = 0f;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -cam.transform.position.z;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 dir = mouseWorld - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }
}