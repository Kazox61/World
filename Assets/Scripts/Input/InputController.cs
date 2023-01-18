using System;
using GameNS.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WorldNS;

namespace ServiceNS {
    public class InputController : MonoBehaviour {

        public InputField inputConfigName;
        public SpriteRenderer previewSprite;
        public SpriteRenderer mouseIndicator;

        private EntityConfig activeConfig;
        
        public string ConfigName => inputConfigName.text;
        public bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
        public Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        public void Start() {
            OnConfigNameChanged();
        }

        public void Update() {
            OnSave();
            OnLeftClick();
            OnRightClick();
            UpdatePosition();
        }

        private void UpdatePosition() {
            if (activeConfig == null) {
                return;
            }
            var position = activeConfig.GetAreaCenter(MouseWorldPosition);
            var blocked = FieldController.instance.IsAreaBlocked(position, activeConfig.sizeX, activeConfig.sizeY);
            if (blocked) {
                previewSprite.color = new Color(1f, 0f, 0f, 0.5f);
            }
            else {
                previewSprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
            previewSprite.transform.localPosition = position;
            mouseIndicator.transform.localPosition = position;
        }

        public void OnConfigNameChanged() {
            var config = EntityConfigCore.GetConfig(ConfigName);
            if (config == null) {
                return;
            }
            activeConfig = config;
            previewSprite.sprite = config.defaultSprite;
            mouseIndicator.size = new Vector2(config.sizeX, config.sizeY);

        }

        public void OnSave() {
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftControl)) {
                FileService.SaveEntities(FieldController.instance.entities);
            }
        }

        public void OnLeftClick() {
            if (IsMouseOverUI) {
                return;
            }
            if (Input.GetMouseButtonDown(0)) {
                FieldController.instance.TryCreateEntity(EntityConfigCore.GetConfig(ConfigName), MouseWorldPosition);
            }
        }

        public void OnRightClick() {
            if (IsMouseOverUI) {
                return;
            }
            if (Input.GetMouseButtonDown(1)) {
                FieldController.instance.TryRemoveEntity(MouseWorldPosition);
            }
        }
    }
}