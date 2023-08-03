using UnityEngine;

namespace Boxey.Bricks.Extras {
    public class CameraMovement : MonoBehaviour {
        private float m_rotationX;
        private float m_rotationY;
        
        [Header("Movement")]
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotationSpeed = 100f;
        

        private void Awake() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                var scale = Time.timeScale == 0 ? 1 : 0;
                Time.timeScale = scale;
                if (scale == 0) {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            float mult = 1;
            if (Input.GetKey(KeyCode.LeftShift)) mult = 10;
            var translationX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime * mult;
            var translationZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime * mult;
            transform.Translate(new Vector3(translationX, 0, translationZ));
            
            m_rotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            m_rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            m_rotationY = Mathf.Clamp(m_rotationY, -90f, 90f);
            transform.rotation = Quaternion.Euler(m_rotationY, m_rotationX, 0f);
        }
    }
}
