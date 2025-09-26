using UnityEngine;
using Utils;

public class DestroyAfterSeconds : MonoBehaviour{
    public float time;
    private Timer timer;
    private void Awake() {
        timer = new Timer(time);
    }

    private void Update() {
        if (timer.HasFinished())
            Destroy(gameObject);
    }
}