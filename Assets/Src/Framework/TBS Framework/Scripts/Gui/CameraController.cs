using UnityEngine;

namespace TbsFramework.Gui
{
    /// <summary>
    /// Simple movable camera implementation.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float ScrollSpeed = 7;
        public float ScrollEdge  = 0.01f;

        void Update()
        {
            float scrollSpeed = ScrollSpeed;
            if (GameSettings.Instance != null && GameSettings.Instance.Preferences != null)
                scrollSpeed *= GameSettings.Instance.Preferences.ScrollSpeed;

            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            {
                transform.Translate(transform.right * Time.deltaTime * scrollSpeed, Space.World);
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
            {
                transform.Translate(transform.right * Time.deltaTime * -scrollSpeed, Space.World);
            }

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            {
                transform.Translate(transform.up * Time.deltaTime * scrollSpeed, Space.World);
            }
            else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
            {
                transform.Translate(transform.up * Time.deltaTime * -scrollSpeed, Space.World);
            }
        }
    }
}