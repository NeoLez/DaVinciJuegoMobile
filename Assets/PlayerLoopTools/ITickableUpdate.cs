namespace NonMonobehaviorUpdates {
    /// <summary>
    /// Objects implementing this interface can receive updates every frame.
    /// </summary>
    public interface ITickableUpdate {
        public void OnUpdate();
    }
}