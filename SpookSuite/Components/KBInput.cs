using UnityEngine;

namespace SpookSuite.Components
{
    internal class KBInput : MonoBehaviour
    {
        public float sprintMultiplier = 2.3f;
        public float movementSpeed = 17f;

        public Vector3 movement;

        private Vector3 forward, right, up;

        public Vector3 Forward
        {
            get => forward == Vector3.zero ? forward = transform.forward : forward;
            set => forward = value;
        }

        public Vector3 Right
        {
            get => right == Vector3.zero ? right = transform.right : right;
            set => right = value;
        }

        public Vector3 Up
        {
            get => up == Vector3.zero ? up = transform.up : up;
            set => up = value;
        }

        public void Configure(Vector3 forward, Vector3 right, Vector3 up)
        {
            this.forward = forward;
            this.right = right;
            this.up = up;
        }

        private void Update()
        {
            if (Cursor.visible) return;
            Vector3 input = new Vector3();

            if(Input.GetKey(KeyCode.W)) input += Forward * movementSpeed;
            if(Input.GetKey(KeyCode.S)) input -= Forward * movementSpeed;
            if(Input.GetKey(KeyCode.A)) input -= Right * movementSpeed;
            if(Input.GetKey(KeyCode.D)) input += Right * movementSpeed;
            if(Input.GetKey(KeyCode.Space)) input += Up * movementSpeed;
            if(Input.GetKey(KeyCode.LeftControl)) input -= Up * movementSpeed;

            if(input.Equals(Vector3.zero))
            {
                movement = Vector3.zero;
                return;
            }

            sprintMultiplier = Input.GetKey(KeyCode.LeftShift) ? Mathf.Min(sprintMultiplier + (5f * Time.deltaTime), 5f) : 1f;
            movement = input;
        }
    }
}
