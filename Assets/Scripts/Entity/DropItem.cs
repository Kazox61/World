using ServiceNS;
using UnityEngine;

namespace GameNS.Entity {
    public class DropItem : MonoBehaviour {
        public static GameObject prefab;

        public DropItemConfig config;
        public SpriteRenderer spriteRenderer;
        
        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad() {
            prefab = Resources.Load<GameObject>("Prefabs/DropItem");
        }
        
        public static DropItem Create(DropItemConfig config, Vector3 position) {
            var dropItem = GameObjectPool.Instance.Get<DropItem>(prefab);
            dropItem.config = config;
            dropItem.Initialize(position);
            return dropItem;
        }

        public void Initialize(Vector3 position) {
            transform.position = position;
            spriteRenderer.sprite = config.sprite;
            Spread();
        }

        public void Spread() {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(x, y).normalized * 5;
        }
    }
}