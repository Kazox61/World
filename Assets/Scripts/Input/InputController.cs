using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WorldBuilderNs {
	public class InputController : MonoBehaviour {
		public static InputController Instance { get; private set; }


		public event Action OnLeftMouseStarted;
		public event Action OnLeftMouseCanceled;
		public event Action OnRightMouse;
		public event Action<Vector2> OnPositionMouse;
		public event Action<float> OnScrollWheel;
		public event Action<Vector2> OnMovementKeyboard;
		public event Action OnKeyDownKeyboardK;
		public event Action OnKeyDownKeyboardP;

		private Vector2 lastMousePosition = Vector2.zero;

		public bool isLeftMousePressed = false;
		public bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();

		public void Awake() {
			Instance = this;
		}

		public void Update() {
			CheckLeftMouse();
			CheckRightMouse();
			CheckScrollWheelMouse();
			CheckPositionMouse();
			CheckMovementKeyboard();
			CheckKeyDownKeyboard(KeyCode.K, OnKeyDownKeyboardK);
			CheckKeyDownKeyboard(KeyCode.P, OnKeyDownKeyboardP);
		}

		private void CheckPositionMouse() {
			if (IsMouseOverUI) {
				return;
			}

			var mousePosition = (Vector2)Input.mousePosition;
			if (mousePosition.Equals(lastMousePosition)) {
				OnPositionMouse?.Invoke(mousePosition);
			}

			lastMousePosition = mousePosition;
		}

		private void CheckLeftMouse() {
			if (Input.GetMouseButtonUp(0) && isLeftMousePressed) {
				isLeftMousePressed = false;
				OnLeftMouseCanceled?.Invoke();
			}

			if (IsMouseOverUI) {
				return;
			}

			if (Input.GetMouseButtonDown(0)) {
				isLeftMousePressed = true;
				OnLeftMouseStarted?.Invoke();
			}
		}

		private void CheckRightMouse() {
			if (IsMouseOverUI) {
				return;
			}

			if (Input.GetMouseButtonDown(1)) {
				OnRightMouse?.Invoke();
			}
		}

		private void CheckScrollWheelMouse() {
			var scrollDelta = Input.mouseScrollDelta.y;
			if (scrollDelta != 0) {
				OnScrollWheel?.Invoke(scrollDelta);
			}
		}

		private void CheckKeyDownKeyboard(KeyCode keyCode, Action action) {
			if (Input.GetKeyDown(keyCode)) {
				action.Invoke();
			}
		}

		private void CheckMovementKeyboard() {
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			if (x == 0f && y == 0f) {
				return;
			}

			OnMovementKeyboard?.Invoke(new Vector2(x, y));
		}
	}
}