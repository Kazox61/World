using UnityEngine;

namespace SetupNS {
    public static class SetupCollectionLoader {
        
        public static SetupCollectionActor SetupCollectionActor { get; private set; }
        public static SetupCollectionEntity SetupCollectionEntity { get; private set; }
        public static SetupCollectionTerrain SetupCollectionTerrain { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad() {
            SetupCollectionActor = Resources.Load<SetupCollectionActor>("SetupCollections/SetupCollectionActor");
            SetupCollectionEntity = Resources.Load<SetupCollectionEntity>("SetupCollections/SetupCollectionEntity");
            SetupCollectionTerrain = Resources.Load<SetupCollectionTerrain>("SetupCollections/SetupCollectionTerrain");
        }
    }
}