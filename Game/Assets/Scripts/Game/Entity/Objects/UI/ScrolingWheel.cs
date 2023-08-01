using UnityEngine;
using UnityEngine.UI;
using Game.Data;
using Game.Entity.Tank;

namespace Game.Ui
{
    public class ScrolingWheel : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private GameObject selectedSegment;
        [SerializeField] private RectTransform selectedSegmentFillArea;

        private float segmentAngle;

        private void Awake() {
            segmentAngle = 360 / items.Length;
            SetupItems();
            SetupSelectedSegment();
        }

        private void Update() {
            RotateSelectionWheel();
        }

        private void RotateSelectionWheel() {
            Vector3 mousePos = InputSystem.TankControler.GetMousePosition();
            Vector3 loockPos = (mousePos - transform.position).normalized / 2;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(loockPos.x, 90, loockPos.z));
            selectedSegment.transform.rotation = rotation;
        }

        private void SetupItems() {
            for (int i = 0; i < items.Length; i++) {
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