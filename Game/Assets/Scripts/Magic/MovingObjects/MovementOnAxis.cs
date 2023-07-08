using UnityEngine;

namespace Magic
{
    public enum ShiftMode { non, xAxis, yAxis, zAxis, xzAxis, random }
    public enum ShiftAxis { xAxis, yAxis, zAxis }

    public class MovementOnAxis : MonoBehaviour {
        public Vector3 startPos { get; private set; }
        public Vector3[] targetX { get; private set; } = new Vector3[2];
        public Vector3[] targetY { get; private set; } = new Vector3[2];
        public Vector3[] targetZ { get; private set; } = new Vector3[2];

        private int indexX = 0;
        private int indexY = 0;
        private int indexZ = 0;

        [HideInInspector] public float speed;

        [HideInInspector] public float DistanceX;
        [HideInInspector] public float DistanceY;
        [HideInInspector] public float DistanceZ;

        [HideInInspector] public bool MoveX;
        [HideInInspector] public bool MoveY;
        [HideInInspector] public bool MoveZ;
        [HideInInspector] public AnimationCurve CurveX;
        [HideInInspector] public AnimationCurve CurveY;
        [HideInInspector] public AnimationCurve CurveZ;
        [HideInInspector] public AnimationCurve startShiftCurve;

        [HideInInspector] public ShiftMode ShiftedStart = ShiftMode.non;
        [HideInInspector] public ShiftAxis ShiftedAxis = ShiftAxis.yAxis;

        public void DoReset() {
            DistanceX = Mathf.Abs(DistanceX);
            DistanceY = Mathf.Abs(DistanceY);
            DistanceZ = Mathf.Abs(DistanceZ);
            startPos = transform.position;
            CalculateTargets();
        }

        private void Awake() {
            DistanceX = Mathf.Abs(DistanceX);
            DistanceY = Mathf.Abs(DistanceY);
            DistanceZ = Mathf.Abs(DistanceZ);
            startPos = transform.position;
            CalculateTargets();
            SetStartShift();
        }

        private void Update() {
            Vector3 pos = transform.position;
            Vector3 target = pos;

            if (MoveX) {
                target = new Vector3(pos.x + SpeedCurve(pos.x, targetX[indexX].x, DistanceX, indexX, CurveX) * Time.deltaTime, target.y, target.z);
                if (ReachTarget(pos.x, startPos.x, DistanceX)) indexX = SwichIndex(pos.x, startPos.x);
            }
            if (MoveY) {
                target = new Vector3(target.x, pos.y + SpeedCurve(pos.y, targetY[indexY].y, DistanceY, indexY, CurveY) * Time.deltaTime, target.z);
                if (ReachTarget(pos.y, startPos.y, DistanceY)) indexY = SwichIndex(pos.y , startPos.y);
            }
            if (MoveZ) {
                target = new Vector3(target.x, target.y, pos.z + SpeedCurve(pos.z, targetZ[indexZ].z, DistanceZ, indexZ, CurveZ) * Time.deltaTime);
                if (ReachTarget(pos.z, startPos.z, DistanceZ)) indexZ = SwichIndex(pos.z, startPos.z);
            }

            if (MoveY || MoveX || MoveZ) {
                transform.position = Vector3.MoveTowards(pos, target, speed / 512);
            }
        }

        private bool ReachTarget(float pos, float startPos, float distance) {
            return MathM.Difference(pos, startPos) >= distance;
        }

        private float SpeedCurve(float pos, float targetPos, float distance, int index, AnimationCurve Curve) {
            float posIn01 = (1 - (MathM.Difference(pos, targetPos)) / distance / 2);
            return index == 0 ? Curve.Evaluate(posIn01) : -Curve.Evaluate(posIn01);
        }

        private int SwichIndex(float pos, float startPos) {
            // 1 is the negative position
            return pos > startPos ? 1 : 0;
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
                    shift = startShiftCurve.Evaluate(Random.Range(10000f, 10000f));
                    break;
            }
            Vector3 newPos = transform.position;
            Debug.Log(shift);

            switch (ShiftedAxis) {
                case ShiftAxis.xAxis:
                    newPos = new Vector3(startPos.x + shift, startPos.y, startPos.z);
                    break;
                case ShiftAxis.yAxis:
                    newPos = new Vector3(startPos.x, startPos.y + shift, startPos.z);
                    break;
                case ShiftAxis.zAxis:
                    newPos = new Vector3(startPos.x, startPos.y, startPos.z + shift);
                    break;
            }
            transform.position = newPos;
        }

        public void CalculateTargets() {
            targetX[0] = new Vector3(startPos.x + DistanceX, startPos.y, startPos.z);
            targetX[1] = new Vector3(startPos.x - DistanceX, startPos.y, startPos.z);

            targetY[0] = new Vector3(startPos.x, startPos.y + DistanceY, startPos.z);
            targetY[1] = new Vector3(startPos.x, startPos.y - DistanceY, startPos.z);

            targetZ[0] = new Vector3(startPos.x, startPos.y, startPos.z + DistanceZ);
            targetZ[1] = new Vector3(startPos.x, startPos.y, startPos.z - DistanceZ);
        }
    }
}