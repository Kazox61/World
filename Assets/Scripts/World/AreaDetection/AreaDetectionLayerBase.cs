namespace WorldNS {
    public abstract class AreaDetectionLayerBase {
        public bool ignore = false;
        public abstract bool IsClean(DetectionSet detectionSet);
    } 
}