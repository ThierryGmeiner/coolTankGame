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

        private readonly Vector3 DEFAULT_SCALE = new Vector3(0.15f, 0.15f, 0.15f);
        private readonly Vector3 SELECTED_SCALE = new Vector3(0.23f, 0.23f, 0.23f);

        private int segmentCount => items.Length;

        private void Awake() {
            segmentAngle = 360 / segmentCount;
            SetupItems();
            SetupSelectedSegment();
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(90, 0, 0);
            
            selectedSegmentIndex = GetSegmentIndex();
            RotateSelectionWheel();
            HighlightSelectedItem();
        }

        private void RotateSelectionWheel() {
            float finalAngle = selectedSegmentIndex * segmentAngle;
            selectorSlider.transform.rotation = Quaternion.Euler(new(-90, 0, finalAngle));
        }

        private void HighlightSelectedItem() {
            for (int i = 0; i < segmentCount; i++) {
                if (i == selectedSegmentIndex) {
                    segments[i].image.sprite = items[i].SpriteWhenSelected;
                    segments[i].rect.localScale = SELECTED_SCALE;
                } else {
                    segments[i].image.sprite = items[i].Sprite;
                    segments[i].rect.localScale = DEFAULT_SCALE;
                }
            }
        }
        
        private int GetSegmentIndex() {
            float angle = GetAngle();
            for (int i = 0; i < segmentCount; i++) {
                if (angle < ( ( i + 1 ) * segmentAngle ) - ( segmentAngle / 2 ) ) {
                    return i;
                }
            } return 0;
        }

        private float GetAngle() {
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
                GameObject canvas = InstantiateItemCanvas(items[i].Sprite, angle);
                canvas.name = "itam_" + i;
                segments[i] = new(items[i], canvas);                
            }
        }

        private GameObject InstantiateItemCanvas(Sprite sprite, float angle) {
            // instantiate image and helper
            GameObject midPoint = new GameObject();
            GameObject segment = ScrolingWheelSegment.EmptyCanvas();
            midPoint.transform.SetParent(transform);
            midPoint.transform.position = transform.position;
            segment.transform.SetParent(midPoint.transform);

            // mutate item
            RectTransform rect = segment.GetComponent<RectTransform>();
            rect.localScale = DEFAULT_SCALE;
            rect.localPosition = new Vector3(0, 3.5f, 0);
            midPoint.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            segment.transform.SetParent(transform);
            rect.localRotation = Quaternion.Euler(0, 0, 0);
            Destroy(midPoint);
            segment.GetComponent<Image>().sprite = sprite;

            return segment;
        }
    }
}