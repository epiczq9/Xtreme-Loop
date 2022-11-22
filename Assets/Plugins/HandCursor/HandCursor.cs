using UnityEngine;
using UnityEngine.UI;

namespace Aezakmi
{
    public class HandCursor : MonoBehaviour
    {
        [SerializeField] private KeyCode ToggleVisibleKey = KeyCode.F;
        [SerializeField] private GameObject Pointer;
        [SerializeField] private GameObject PointerPressed;

        private bool _isPressed;
        private bool _isDisplayed;
        private Vector3 _cursorOffset;

        private void Start()
        {
            Pointer.gameObject.SetActive(false);
            _cursorOffset = new Vector3(1f, -1f, 0f) * Pointer.GetComponent<RectTransform>().sizeDelta / 2;
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(ToggleVisibleKey))
            {
                _isDisplayed = !_isDisplayed;
                Cursor.visible = !Cursor.visible;
            }

            if (!_isDisplayed)
            {
                Pointer.gameObject.SetActive(false);
                PointerPressed.gameObject.SetActive(false);
                return;
            }

            Pointer.gameObject.transform.position = Input.mousePosition + _cursorOffset;
            PointerPressed.gameObject.transform.position = Input.mousePosition + _cursorOffset;

            _isPressed = Input.GetMouseButton(0);

            if (_isPressed)
            {
                Pointer.gameObject.SetActive(false);
                PointerPressed.gameObject.SetActive(true);
            }
            else if (!_isPressed)
            {
                Pointer.gameObject.SetActive(true);
                PointerPressed.gameObject.SetActive(false);
            }
        }
    }
}