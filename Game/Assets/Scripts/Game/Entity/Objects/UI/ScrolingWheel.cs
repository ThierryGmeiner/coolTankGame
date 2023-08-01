using UnityEngine;
using UnityEngine.UI;
using Game.Data;

namespace Game.Ui
{
    public class ScrolingWheel : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private GameObject selectedSegment;
        [SerializeField] private RectTransform selectedSegmentFillArea;
        private float segmentAngle;

        private int segmentCount => items.Length;

        private void Awake() {
            segmentAngle = 360 / segmentCount;
            SetupItems();
            SetupSelectedSegment();
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(90, 0, 0);
            RotateSelectionWheel();
        }

        private void RotateSelectionWheel() {
            Vector3 loockPos = InputSystem.TankControler.GetMousePosition() - transform.position;
            float rotationAngle = Quaternion.LookRotation(new Vector3(loockPos.x, 90, loockPos.z)).eulerAngles.y;
            float finalAngle = GetSegmentIndex(rotationAngle) * segmentAngle;
            selectedSegment.transform.rotation = Quaternion.Euler(new(-90, 0, finalAngle));
        }

        private int GetSegmentIndex(float angle) {
            for (int i = 0; i < segmentCount; i++) {
                if (angle < ( ( i + 1 ) * segmentAngle ) - ( segmentAngle / 2 ) ) {
                    return i;
                }
            } return 0;
        }

        private void GetTargetedItem() {
            //Debug.Log()
        }

        private void SetupItems() {
            for (int i = 0; i < segmentCount; i++) {
                float angle = i * segmentAngle;
                InstantiateItemCanvas(items[i].Sprite, angle);
            }
        }

        private void SetupSelectedSegment() {
            selectedSegment.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Slider slider = selectedSegment.GetComponent<Slider>();
            slider.value = segmentAngle / 360;
            selectedSegmentFillArea.localRotation = Quaternion.Euler(0, 0, segmentAngle / 2);
        }

        private void InstantiateItemCanvas(Sprite image, float angle) {
            // instantiate image and helper
            GameObject midPoint = new GameObject();
            GameObject segment = EmptyCanvas();
            midPoint.transform.SetParent(transform);
            midPoint.transform.position = transform.position;
            segment.transform.SetParent(midPoint.transform);

            // mutate item
            segment.GetComponent<Image>().sprite = image;
            RectTransform rect = segment.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            rect.localPosition = new Vector3(0, 3.5f, 0);
            midPoint.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            segment.transform.SetParent(transform);
            rect.localRotation = Quaternion.Euler(0, 0, 0);
            Destroy(midPoint);
        }

        private static GameObject EmptyCanvas() {
            GameObject canvas = new GameObject();
            RectTransform rect = canvas.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(10, 10);
            canvas.AddComponent<CanvasRenderer>();
            canvas.AddComponent<Image>();
            return canvas;
        }
    }
}