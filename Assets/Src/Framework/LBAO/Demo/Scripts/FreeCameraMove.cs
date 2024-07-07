using UnityEngine;
using System.Collections;

namespace LBAOFX
{
	public class FreeCameraMove : MonoBehaviour
	{
		public float cameraSensitivity = 150;

		float rotationX = 0.0f;
		float rotationY = 0.0f;
		Quaternion originalRotation;

		void Start ()
		{
			Cursor.lockState = CursorLockMode.Locked;
			originalRotation = Camera.main.transform.rotation;
		}

		void Update ()
		{
			Vector2 mousePos = Input.mousePosition;
			if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
				return;

			rotationX += Input.GetAxis ("Mouse X") * cameraSensitivity * Time.deltaTime;
			rotationX = Mathf.Clamp (rotationX, -10, 10);
			rotationY += Input.GetAxis ("Mouse Y") * cameraSensitivity * Time.deltaTime;
			rotationY = Mathf.Clamp (rotationY, -10, 10);

			transform.localRotation = Quaternion.AngleAxis (rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis (rotationY, Vector3.left);
			transform.localRotation *= originalRotation;

		}

	}
}