using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace ServiceNS {
    public class PixelPerfectZoom : MonoBehaviour {
        public Camera mainCamera;
        public PixelPerfectCamera pixelPerfectCamera;
        
        public float scale = 1;
        private float currentScale;
        private float speed = 100;
        private float minScale = 1;
        private float maxScale = 10;

        private int screenHeight;
        private int screenWidth;

        private void Start() {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            mainCamera.orthographicSize = 20;
        }

        public void Zoom(int direction) {
            scale += direction * Time.deltaTime * speed;
            scale = Mathf.Clamp(scale, minScale, maxScale);
            if (Mathf.Max(scale, currentScale) - Mathf.Min(scale, currentScale) >= 1f) {
                currentScale = Mathf.Round(scale);
                pixelPerfectCamera.refResolutionX = Mathf.RoundToInt(screenWidth / currentScale);
                pixelPerfectCamera.refResolutionY = Mathf.RoundToInt(screenHeight / currentScale);
            }
        }
    }
}