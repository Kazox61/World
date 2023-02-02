using System.Collections.Generic;
using SetupNS;
using UnityEngine;
using WorldNS;

namespace GameNS.WorldEditor {
    using GameNS;
    public class PaintBrushSimple: PaintBrushBase {
        private Stack<List<Entity>> undoList = new ();
        private List<Entity> createdEntities = new();

        public PaintBrushSimple(InputWorldEditor worldEditor) : base(worldEditor) { }

        public override void OnEnter() {
            base.OnEnter();
        }

        public override void OnLeave() {
            base.OnLeave();
        }

        protected override void LeftMouseStarted() {
            
        }

        protected override void LeftMouseCanceled() {
            undoList.Push(createdEntities);
            createdEntities = new List<Entity>();
        }

        protected override void MouseMoved(Vector2 position) {
            var currentWorldMousePosition = worldEditor.mainCamera.ScreenToWorldPoint(position);
            
            UpdateMouseIndicator(currentWorldMousePosition);
            UpdatePreview(currentWorldMousePosition);
            if (!inputController.isLeftMousePressed) {
                return;
            }

            var field = GridHelper.PositionToField(currentWorldMousePosition);
            SetupTerrain.CreateTerrain(worldEditor.currentSetupTerrain, field);
            //Environment.Instance.Place(currentWorldMousePosition);
            //if (success) {
            //  createdEntities.Add(entity);
            //}
        }

        public override void Undo() {
            var entities = undoList.Pop();
            foreach (var entity in entities) {
                //EntityHelper.Instance.RemoveEntity(entity);
            }
        }
    }
}