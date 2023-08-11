using UnityEngine;
using UnityEngine.UI;
using Game.Data;

namespace Game.UI
{
    public class ScrolingWheel : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private GameObject selectorSlider;
        [SerializeField] private RectTransform selectorFillArea;
        private ScrolingWheelSegment[] segments;
        private float segmentAngle;
        
        private int selectedSegmentIndex = 0;
        private int oldSelectedSegmentIndex = 0;

        public event System.Action<Item> OnSelectItem;

        private int segmentCount => items.Length;

        private void Awake() {
            segmentAngle = 360 / segmentCount;
            SetupItems();
            SetupSelectedSegment();
            gameObject.SetActive(false);
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(90, 0, 0);
            
            selectedSegmentIndex = GetSegmentIndex();
            RotateSelectionWheel();
            HighlightSelectedItem();
        }

        public void SelectItem() {
            OnSelectItem?.Invoke(items[selectedSegmentIndex]);
        }

        private void RotateSelectionWheel() {
            float finalAngle = selectedSegmentIndex * segmentAngle;
            selectorSlider.transform.rotation = Quaternion.Euler(new(-90, 0, finalAngle));
        }

        private void HighlightSelectedItem() {
            if (selectedSegmentIndex != oldSelectedSegmentIndex) {
                segments[selectedSegmentIndex].SelectItem();
                segments[oldSelectedSegmentIndex].DeselectItem();
            }
        }
        
        private int GetSegmentIndex() {
            oldSelectedSegmentIndex = selectedSegmentIndex;
            float angle = GetSelectorAngle();

            for (int i = 0; i < segmentCount; i++) {
                if (angle < ((i + 1) * segmentAngle) - (segmentAngle / 2)) {
                    return i;
                }
            } return 0;
        }

        private float GetSelectorAngle() {
            Vector3 loockPos = InputSystem.TankControler.GetMousePosition() - transform.position;
            return Quaternion.LookRotation(new Vector3(loockPos.x, 90, loockPos.z)).eulerAngles.y;
        }

        private void SetupSelectedSegment() {
            selectorSlider.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Slider slider = selectorSlider.GetComponent<Slider>();
            slider.value = segmentAngle / 360;
            selectorFillArea.localRotation = Quaternion.Euler(0, 0, segmentAngle / 2);
        }

        private void SetupItems() {
            segments = new ScrolingWheelSegment[segmentCount];
            for (int i = 0; i < segmentCount; i++) {
                float angle = i * segmentAngle;
                GameObject canvas = ScrolingWheelSegment.InstantiateItemCanvas(transform, angle);
                segments[i] = new(items[i], canvas);
                canvas.name = $"item:{items[i].type.ToString()}-ID:{(int)items[i].type}";
            }
        }
    }
}