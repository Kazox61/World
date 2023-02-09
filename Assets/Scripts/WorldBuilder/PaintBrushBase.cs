using UnityEngine;
using WorldNS;

namespace WorldBuilderNs {
    public abstract class PaintBrushBase {
        protected InputController inputController;
        protected InputWorldBuilder worldBuilder;

        protected PaintBrushBase(InputWorldBuilder worldBuilder) {
            inputController = InputController.Instance;
            this.worldBuilder = worldBuilder;
        }

        public virtual void OnEnter() {
            inputController.OnLeftMouseStarted += LeftMouseStarted;
            inputController.OnLeftMouseCanceled += LeftMouseCanceled;
            inputController.OnPositionMouse += MouseMoved;
        }

        public virtual void OnUpdate() {
            
        }

        public virtual void OnLeave() {
            inputController.OnLeftMouseStarted -= LeftMouseStarted;
            inputController.OnLeftMouseCanceled -= LeftMouseCanceled;
            inputController.OnPositionMouse -= MouseMoved;
        }

        protected virtual void LeftMouseStarted() {
            
        }

        protected virtual void LeftMouseCanceled() {
            
        }

        protected virtual void MouseMoved(Vector2 position) {
            
        } 
        
        protected void UpdateMouseIndicator(Vector2 worldPosition) {
            var field = GridHelper.PositionToField(worldPosition);
            worldBuilder.mouseIndicator.transform.localPosition = GridHelper.FieldToPosition(field);
            
        }

        protected void UpdatePreview(Vector2 worldPosition) {
            /*
            if (Environment.Instance.entitySetup == null && Environment.Instance.terrainConfig == null) {
                return;
            }
            Vector2 position;
            if (Environment.Instance.entitySetup != null) {
                position = Environment.Instance.entitySetup.GetAreaCenter(worldPosition);
            }
            else {
                position = ChunkHelper.PositionToFieldPosition(worldPosition) + Vector2.one * 0.5f;
            }
            var blocked = FieldController.Instance.IsAreaBlocked(position, worldEditor.ActiveConfig.sizeX, worldEditor.ActiveConfig.sizeY);
            if (blocked) {
                worldEditor.previewSprite.color = new Color(1f, 0f, 0f, 0.5f);
            }
            else {
                worldEditor.previewSprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
            worldEditor.previewSprite.transform.localPosition = position;
            */
        }

        public virtual void Undo() {
            
        }
    }
}