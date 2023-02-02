using WorldNS;

namespace SaveSystemNS {
    public class SaveSystem {
        public static SaveSystem Instance = new();
        private World world;

        public World World {
            get {
                if (world == null) {
                    LoadWorld();
                }
                return world;
            }
        }

        private void LoadWorld() {
            world = new World();
            world.Create();
        }
    }
}