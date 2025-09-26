using System;
using UnityEngine;

public class InputSystem : MonoBehaviour {
    public static InputSystem Instance {
        get;
        private set;
    }
    private void Awake() {
        Instance = this;
    }

    private readonly CustomTouch[] touches = new CustomTouch[2];

    [SerializeField] private float dragDistance;
    [SerializeField] private float holdTime;

    public event Action<CustomTouch> OnTap;
    public event Action<CustomTouch> OnHold;
    public event Action<CustomTouch> OnDrag;
    
    
    public void Update() {
        if (Input.touchCount > 0) {
            for (int i = 0; i < Input.touchCount && i < 2; i++) {
                
                Touch touch = Input.touches[i];
                switch (touch.phase) {
                    case TouchPhase.Began:
                        touches[i] = new CustomTouch(touch.position);
                        break;
                    case TouchPhase.Moved:
                        touches[i].CurrentPosition = touch.position;
                        if (touches[i].Phase == CustomTouchPhase.Hold) {
                            touches[i].OnPositionChanged?.Invoke(touches[i].CurrentPosition);
                        }
                        if (touches[i].Phase != CustomTouchPhase.Hold && 
                            (touches[i].CurrentPosition - touches[i].InitialPosition).magnitude > dragDistance) {
                            touches[i].Phase = CustomTouchPhase.Drag;
                        }
                        break;
                    case TouchPhase.Stationary:
                        touches[i].CurrentPosition = touch.position;
                        if (touches[i].Phase != CustomTouchPhase.Hold && Time.time - touches[i].TimeStarted > holdTime) {
                            touches[i].Phase = CustomTouchPhase.Hold;
                            OnHold?.Invoke(touches[i]);
                        }
                        break;
                    case TouchPhase.Ended:
                        touches[i].CurrentPosition = touch.position;
                        switch (touches[i].Phase) {
                            case CustomTouchPhase.Tap:
                                OnTap?.Invoke(touches[i]);
                                break;
                            case CustomTouchPhase.Drag:
                                OnDrag?.Invoke(touches[i]);
                                break;
                            case CustomTouchPhase.Hold:
                                touches[i].OnEnded?.Invoke();
                                break;
                        }
                        touches[i].Phase = CustomTouchPhase.Ended;
                        break;
                }
            }
        }
    }

    public class CustomTouch {
        public float TimeStarted;
        public Vector2 InitialPosition;
        public Vector2 CurrentPosition;
        public CustomTouchPhase Phase;
        public Action<Vector2> OnPositionChanged;
        public Action OnEnded;

        public CustomTouch(Vector2 initialPosition, CustomTouchPhase phase = CustomTouchPhase.Tap) {
            TimeStarted = Time.time;
            InitialPosition = initialPosition;
            CurrentPosition = initialPosition;
            Phase = phase;
        }
        
    }

    public enum CustomTouchPhase {
        Tap,
        Drag,
        Hold,
        Ended,
    }
}