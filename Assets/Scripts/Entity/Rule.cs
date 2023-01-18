using UnityEngine;

namespace GameNS.Entity {
    [System.Serializable]
    public class Rule {
        public Neighbor[] neighbors;
        public Sprite[] sprites;
        public Transform ruleTransform;
        public float animationSpeed;
        public float perlinScale;
        public Transform randomTransform;
        public ColliderType colliderType;
        public OutputSprite output;
        public float offsetX;
        public float offsetY;
			
        public Rule()
        {
            neighbors = new Neighbor[8];
            sprites = new Sprite[1];

            for(int i=0; i<neighbors.Length; i++)
                neighbors[i] = Neighbor.DontCare;
        }

        public enum Transform { Fixed, Rotated, MirrorX, MirrorY }
        public enum Neighbor { DontCare, This, NotThis }
        public enum OutputSprite { Single, Random, Animation }
        
        public enum ColliderType { Sprite }
    }
}