namespace SetupNS {
    public static class TerrainHelper {
        public static TerrainRule[] GetRules() {
            return new TerrainRule[] {
                new() { input = new[] { 0, 0, 0, 0, 0, 0, 0, 0, }, output = 0 }
            };
        }
    }
}