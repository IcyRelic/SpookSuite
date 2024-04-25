using UnityEngine;

namespace SpookSuite.Components
{
    internal class MouseInput : MonoBehaviour
    {
        private float Yaw = 0f;
        private float Pitch = 0f;

        private void Update()
        {
            if(Cursor.visible) return;

            Yaw += Input.GetAxis("Mouse X");
            Yaw = (Yaw + 360f) % 360f;

            Pitch -= Input.GetAxis("Mouse Y");
            Pitch = Mathf.Clamp(Pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(Pitch, Yaw, 0f);
        }
    }
}
