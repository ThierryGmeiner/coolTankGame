using UnityEngine;

namespace Magic
{
    public class RotatOnAxis : MonoBehaviour
    {
        [HideInInspector] public float speed;

        [HideInInspector] public bool RotateX;
        [HideInInspector] public bool RotateY;
        [HideInInspector] public bool RotateZ;
        [HideInInspector] public bool RotateClockwiseX = true;
        [HideInInspector] public bool RotateClockwiseY = true;
        [HideInInspector] public bool RotateClockwiseZ = true;
        [HideInInspector] public AnimationCurve CurveX;
        [HideInInspector] public AnimationCurve CurveY;
        [HideInInspector] public AnimationCurve CurveZ;
        [HideInInspector] public AnimationCurve startShiftCurve;

        [HideInInspector] public ShiftMode ShiftedStart = ShiftMode.non;
        [HideInInspector] public ShiftAxis ShiftedAxis = ShiftAxis.yAxis;

        private Quaternion startRotation;
        private float lifeTime = 0;

        private void Start() {
            startRotation = transform.rotation;
            SetStartShift();
        }

        private void Update() {
            lifeTime += Time.deltaTime;

            float targetX = 0;
            float targetY = 0;
            float targetZ = 0;

            if (RotateX) {
                targetX = RotateClockwiseX ? 
                    speed * CurveX.Evaluate(lifeTime) * Time.deltaTime : -(speed * CurveX.Evaluate(lifeTime) * Time.deltaTime);
            }
            if (RotateY) {
                targetY = RotateClockwiseY ? 
                    speed * CurveY.Evaluate(lifeTime) * Time.deltaTime : -(speed * CurveY.Evaluate(lifeTime) * Time.deltaTime);
            }
            if (RotateZ) {
                targetZ = RotateClockwiseZ ? 
                    speed * CurveZ.Evaluate(lifeTime) * Time.deltaTime : -(speed * CurveZ.Evaluate(lifeTime) * Time.deltaTime);
            }

            if (RotateY || RotateX || RotateZ) {
                transform.Rotate(new Vector3(targetX, targetY, targetZ));
            }
        }

        private void SetStartShift() {
            float shift = 0;

            switch (ShiftedStart) {
                case ShiftMode.non:
                    return;
                case ShiftMode.xAxis:
                    shift = startShiftCurve.Evaluate(transform.position.x);
                    break;
                case ShiftMode.yAxis:
                    shift = startShiftCurve.Evaluate(transform.position.y);
                    break;
                case ShiftMode.zAxis:
                    shift = startShiftCurve.Evaluate(transform.position.z);
                    break;
                case ShiftMode.xzAxis:
                    shift = startShiftCurve.Evaluate(transform.position.z + transform.position.x);
                    break;
                case ShiftMode.random:
                    shift = startShiftCurve.Evaluate(Random.Range(float.MinValue, float.MaxValue));
                    break;
            }
            Vector3 newRotation = Vector3.zero;

            switch (ShiftedAxis) {
                case ShiftAxis.xAxis:
                    newRotation = new Vector3(startRotation.x + shift, startRotation.y, startRotation.z);
                    break;
                case ShiftAxis.yAxis:
                    newRotation = new Vector3(startRotation.x, startRotation.y + shift, startRotation.z);
                    break;
                case ShiftAxis.zAxis:
                    newRotation = new Vector3(startRotation.x, startRotation.y, startRotation.z + shift);
                    break;
            }
            transform.Rotate(newRotation);
        }
    }
}