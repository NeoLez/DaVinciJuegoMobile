using NonMonobehaviorUpdates;
using UnityEngine;

namespace DefaultNamespace {
    public class InputTests : ITickableUpdate {
        private static InputTests instance;
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize() {
            instance = new InputTests();
            UpdatesManager.RegisterUpdate(instance);
        }

        public void OnUpdate() {
            foreach (var touch in Input.touches) {
                var a = $@"
                Finger ID: {touch.fingerId}
                Phase: {touch.phase}
                Position: {touch.position}
                Raw Position: {touch.rawPosition}
                Delta Position: {touch.deltaPosition}
                Delta Time: {touch.deltaTime}
                Tap Count: {touch.tapCount}
                ";
                Debug.Log(a);
                //Debug.Log("Type: " + touch.type); KINDA USELESS
                //Debug.Log("Radius: " + touch.radius); NOT VERY RELIABLE AND PROBABLY HEAVILY QUANTIZED
                //Debug.Log("Radius Variance: " + touch.radiusVariance);  DOESNT DO ANYTHING
                //Debug.Log("Altitude Angle: " + touch.altitudeAngle); DOESNT DO ANYTHING
                //Debug.Log("Azimuth Angle: " + touch.azimuthAngle); DOESNT DO ANYTHING
                //Debug.Log("Maximum Pressure: " + touch.maximumPossiblePressure);
                //Debug.Log("Pressure: " + touch.pressure);         PRESSURE IS NOT RELIABLE
            }
        }
    }
}