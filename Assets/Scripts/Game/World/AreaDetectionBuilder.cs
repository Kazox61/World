using System.Collections.Generic;

namespace GameNS {
    public class AreaDetectionBuilder {
        public static AreaDetectionBuilder Instance { get; } = new AreaDetectionBuilder();

        private readonly List<AreaDetectionLayerBase> areaDetectionLayers = new List<AreaDetectionLayerBase>() {
            new AreaDetectionLayerGround(),
            new AreaDetectionLayerDecoration(),
            new AreaDetectionLayerEntity()
        };

        public AreaDetection GetAreaDetection(bool[] ignoreDetectionLayers = null) {
            var areaDetection = new AreaDetection();

            if (ignoreDetectionLayers == null) {
                areaDetection.areaDetectionLayers = areaDetectionLayers;
                return areaDetection;
            }
            
            for (int i = 0; i < ignoreDetectionLayers.Length; i++) {
                var ignore = ignoreDetectionLayers[i];

                if (!ignore) {
                    areaDetection.areaDetectionLayers.Add(areaDetectionLayers[i]);
                }
            }
            

            return areaDetection;
        }

    }
}