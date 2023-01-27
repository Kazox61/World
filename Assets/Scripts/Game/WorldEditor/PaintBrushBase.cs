using System;
using ServiceNS;
using UnityEngine;
using WorldNS;

namespace GameNS.WorldEditor {
    public abstract class PaintBrushBase {
        protected InputController inputController;
        protected InputWorldEditor worldEditor;

        protected PaintBrushBase(InputWorldEditor worldEditor) {
            inputController = InputController.Instance;
            this.worldEditor = worldEditor;
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
            if (worldEditor.ActiveConfig == null) {
                
                return;
            }
            var position = worldEditor.ActiveConfig.GetAreaCenter(worldPosition);
            
            worldEditor.mouseIndicator.transform.localPosition = position;
        }

        protected void UpdatePreview(Vector2 worldPosition) {
            if (worldEditor.ActiveConfig == null) {
                return;
            }
            var position = worldEditor.ActiveConfig.GetAreaCenter(worldPosition);
            var blocked = FieldController.Instance.IsAreaBlocked(position, worldEditor.ActiveConfig.sizeX, worldEditor.ActiveConfig.sizeY);
            if (blocked) {
                worldEditor.previewSprite.color = new Color(1f, 0f, 0f, 0.5f);
            }
            else {
                worldEditor.previewSprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
            worldEditor.previewSprite.transform.localPosition = position;
        }

        public virtual void Undo() {
            
        }
    }
}