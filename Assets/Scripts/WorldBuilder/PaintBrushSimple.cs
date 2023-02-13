using System.Collections.Generic;
using WorldNS.UserInterface.Controllers;
using SetupNS;
using UnityEngine;
using WorldNS;

namespace WorldBuilderNs {
	public class PaintBrushSimple : PaintBrushBase {
		private Stack<List<Entity>> undoList = new();
		private List<Entity> createdEntities = new();

		public PaintBrushSimple(InputWorldBuilder worldBuilder) : base(worldBuilder) { }

		public override void OnEnter() {
			base.OnEnter();
		}

		public override void OnLeave() {
			base.OnLeave();
		}

		protected override void LeftMouseStarted() { }

		protected override void LeftMouseCanceled() {
			undoList.Push(createdEntities);
			createdEntities = new List<Entity>();
		}

		protected override void MouseMoved(Vector2 position) {
			var currentWorldMousePosition = worldBuilder.mainCamera.ScreenToWorldPoint(position);

			UpdateMouseIndicator(currentWorldMousePosition);
			UpdatePreview(currentWorldMousePosition);
			if (!inputController.isLeftMousePressed) {
				return;
			}

			var field = GridHelper.PositionToField(currentWorldMousePosition);
			var setup = ControllerInventory.instance.activeSlot.item.itemSetup.placeAction.entity;
			Entity.PlaceEntity(setup, field);
		}

		public override void Undo() {
			var entities = undoList.Pop();
			foreach (var entity in entities) {
				//EntityHelper.Instance.RemoveEntity(entity);
			}
		}
	}
}