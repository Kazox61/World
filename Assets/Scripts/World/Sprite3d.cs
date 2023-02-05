using UnityEngine;

namespace WorldNS {
    public class Sprite3d : MonoBehaviour {
        private const int PixelsPerUnit = 16;
        public SpriteRenderer spriteRenderer;
        public int pixelOffset;
        private int zeroLineHeight = 0; // in Units, a position.y

        public void UpdateSpriteOrder() {
            var sprite = spriteRenderer.sprite;
            
            var positionY = (int)((transform.position.y - zeroLineHeight) * PixelsPerUnit);
            var spriteHeight = (int)(sprite.pivot.y / sprite.texture.height * PixelsPerUnit);
            
            var sortingOrder = -positionY + spriteHeight - pixelOffset;

            spriteRenderer.sortingOrder = sortingOrder;
        }

        public void Start() {
            UpdateSpriteOrder();
        }
    }
}