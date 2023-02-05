using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupCollection", menuName = "SetupCollection", order = 0)]
    public class SetupCollection : ScriptableObject {
        public TerrainSetup[] terrainSetups;
        public EntitySetup[] entitySetups;
        public ActorSetup[] actorSetups;
    }
}