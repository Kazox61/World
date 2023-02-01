﻿using ServiceNS;
using SetupNS;
using TMPro;
using UnityEngine;
using GameNS;

namespace GameNS.WorldEditor {
    public class InputWorldEditor : MonoBehaviour {
        public TMP_InputField inputConfigName;
        public SpriteRenderer previewSprite;
        public SpriteRenderer mouseIndicator;
        public PixelPerfectZoom pixelPerfectZoom;
        public Camera mainCamera;

        public float cameraMovementSpeed = 50f;
        
        private InputController inputController;

        private PaintBrushStateMachine paintBrushStateMachine;
        private PaintBrushBase activePaintBrush;
        private PaintBrushSimple paintBrushSimple;
        private PaintBrushLinear paintBrushLinear;
        private PaintBrushRectangular paintBrushRectangular;

        public SetupTerrain currentSetupTerrain;
        
        public string ConfigName => inputConfigName.text;
        
        private void Start() {
            inputController = InputController.Instance;
            
            BuildPaintBrushStateMachine();
            
            SubscribeInputEvents();
            OnConfigNameChanged();
        }

        private void Update() {
            paintBrushStateMachine.CurrentState.OnUpdate();
        }

        private void BuildPaintBrushStateMachine() {
            paintBrushStateMachine = new PaintBrushStateMachine();

            paintBrushSimple = new PaintBrushSimple(this);
            paintBrushLinear = new PaintBrushLinear(this);
            paintBrushRectangular = new PaintBrushRectangular(this);
            
            paintBrushStateMachine.EnterState(paintBrushSimple);
        }
        
        private void SubscribeInputEvents() {
            inputController.OnScrollWheel += CameraZoom;
            inputController.OnKeyDownKeyboardK += () => {
                paintBrushStateMachine.CurrentState.Undo();
            };
            inputController.OnMovementKeyboard += direction => {
                mainCamera.transform.position += (Vector3)(Time.deltaTime * cameraMovementSpeed * direction);
            };
            inputController.OnKeyDownKeyboardP += () => {
                ChunkLoader.Save(ChunkManager.Instance.chunks);
            };
        }

        public void OnConfigNameChanged() {
            var setup = SetupCollectionLoader.SetupCollectionTerrain.GetSetup(ConfigName);
            if (setup != null) {
                currentSetupTerrain = setup;
            }
        }

        private void CameraZoom(float delta) {
            if (delta > 0) {
                pixelPerfectZoom.Zoom(1);
            }
            else {
                pixelPerfectZoom.Zoom(-1);
            }
        }
    }
}