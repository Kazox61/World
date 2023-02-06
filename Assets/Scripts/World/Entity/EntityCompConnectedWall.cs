using System.Collections.Generic;
using SetupNS;

namespace WorldNS {
    public class EntityCompConnectedWall: EntityCompConnectedBase {
        protected override IEnumerable<Rule> GetRules() {
            return new Rule[] {
                new() { input = new[] { 0, 2, 0, 1, 2, 1, 1, 0 }, output = 0 },
                new() { input = new[] { 0, 2, 0, 2, 1, 0, 1, 1 }, output = 1 },
                new() { input = new[] { 1, 1, 2, 1, 1, 1, 1, 1 }, output = 2 },
                new() { input = new[] { 2, 1, 1, 1, 1, 1, 1, 1 }, output = 3 },
                new() { input = new[] { 1, 1, 0, 1, 2, 1, 1, 0 }, output = 4 },
                new() { input = new[] { 0, 1, 1, 2, 1, 0, 1, 1 }, output = 5 },

                new() { input = new[] { 1, 1, 0, 1, 2, 0, 2, 0 }, output = 6 },
                new() { input = new[] { 0, 1, 1, 2, 1, 0, 2, 0 }, output = 7 },
                new() { input = new[] { 1, 1, 1, 1, 1, 1, 1, 2 }, output = 8 },
                new() { input = new[] { 1, 1, 1, 1, 1, 2, 1, 1 }, output = 9 },
                new() { input = new[] { 1, 1, 1, 1, 1, 0, 2, 0 }, output = 10 },
                new() { input = new[] { 0, 2, 0, 1, 1, 1, 1, 1 }, output = 11 },

                new() { input = new[] { 0, 2, 0, 1, 1, 2, 1, 2 }, output = 12 },
                new() { input = new[] { 0, 1, 2, 2, 1, 0, 1, 2 }, output = 13 },
                new() { input = new[] { 0, 1, 0, 2, 2, 0, 1, 0 }, output = 14 },
                new() { input = new[] { 0, 2, 0, 1, 1, 0, 2, 0 }, output = 15 },
                new() { input = new[] { 0, 1, 0, 2, 2, 0, 2, 0 }, output = 16 },
                new() { input = new[] { 0, 2, 0, 1, 2, 0, 2, 0 }, output = 17 },

                new() { input = new[] { 2, 1, 0, 1, 2, 2, 1, 0 }, output = 18 },
                new() { input = new[] { 2, 1, 2, 1, 1, 0, 2, 0 }, output = 19 },
                new() { input = new[] { 0, 2, 0, 2, 2, 0, 2, 0 }, output = 20 },
                new() { input = new[] { 2, 1, 2, 1, 1, 2, 1, 2 }, output = 21 },
                new() { input = new[] { 0, 2, 0, 2, 2, 0, 1, 0 }, output = 22 },
                new() { input = new[] { 0, 2, 0, 2, 1, 0, 2, 0 }, output = 23 },

                new() { input = new[] { 0, 2, 0, 1, 2, 2, 1, 0 }, output = 24 },
                new() { input = new[] { 0, 2, 0, 2, 1, 0, 1, 2 }, output = 25 },
                new() { input = new[] { 2, 1, 2, 1, 1, 1, 1, 2 }, output = 26 },
                new() { input = new[] { 2, 1, 2, 1, 1, 2, 1, 1 }, output = 27 },
                new() { input = new[] { 1, 1, 1, 1, 1, 2, 1, 2 }, output = 28 },
                new() { input = new[] { 1, 1, 2, 1, 1, 1, 1, 2 }, output = 29 },

                new() { input = new[] { 2, 1, 0, 1, 2, 0, 2, 0 }, output = 30 },
                new() { input = new[] { 0, 1, 2, 2, 1, 0, 2, 0 }, output = 31 },
                new() { input = new[] { 1, 1, 2, 1, 1, 2, 1, 2 }, output = 32 },
                new() { input = new[] { 2, 1, 1, 1, 1, 2, 1, 2 }, output = 33 },
                new() { input = new[] { 2, 1, 1, 1, 1, 2, 1, 1 }, output = 34 },
                new() { input = new[] { 1, 2, 1, 2, 2, 2, 2, 2 }, output = 35 },

                new() { input = new[] { 0, 2, 0, 1, 1, 1, 1, 2 }, output = 36 },
                new() { input = new[] { 0, 2, 0, 1, 1, 2, 1, 1 }, output = 37 },
                new() { input = new[] { 2, 1, 0, 1, 2, 1, 1, 0 }, output = 38 },
                new() { input = new[] { 0, 1, 2, 2, 1, 0, 1, 1 }, output = 39 },
                new() { input = new[] { 1, 1, 2, 1, 1, 2, 1, 1 }, output = 40 },
                new() { input = new[] { 2, 1, 1, 1, 1, 1, 1, 2 }, output = 41 },

                new() { input = new[] { 1, 1, 2, 1, 1, 0, 2, 0 }, output = 42 },
                new() { input = new[] { 2, 1, 1, 1, 1, 0, 2, 0 }, output = 43 },
                new() { input = new[] { 1, 1, 0, 1, 2, 2, 1, 0 }, output = 44 },
                new() { input = new[] { 0, 1, 1, 2, 1, 0, 1, 2 }, output = 45 },
                new() { input = new[] { 1, 1, 1, 1, 1, 1, 1, 1 }, output = 46 }

            };
        }
    }
}