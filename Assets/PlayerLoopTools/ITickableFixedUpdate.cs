namespace NonMonobehaviorUpdates {
    /// <summary>
    /// Objects implementing this interface can receive updates every physics calculation.
    /// </summary>
    public interface ITickableFixedUpdate {
        public void OnFixedUpdate();
    }
}