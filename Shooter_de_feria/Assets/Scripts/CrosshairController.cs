using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] float depthFromCamera = 10f;

    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        float x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        float y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        Vector3 sp = new Vector3(x, y, depthFromCamera);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(sp);
        transform.position = worldPos;
    }
}
