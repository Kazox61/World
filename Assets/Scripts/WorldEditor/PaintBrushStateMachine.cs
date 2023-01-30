namespace GameNS.WorldEditor {
    public class PaintBrushStateMachine {
        public PaintBrushBase CurrentState { get; private set; }

        public void EnterState(PaintBrushBase brush) {
            CurrentState?.OnLeave();
            CurrentState = brush; 
            CurrentState.OnEnter();
        }
    }
}