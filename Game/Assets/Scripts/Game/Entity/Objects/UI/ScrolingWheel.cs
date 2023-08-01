using UnityEngine;
using UnityEngine.UI;
using Game.Data;

namespace Game.Ui
{
    public class ScrolingWheel : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private Texture2D selectedSegment;
        [SerializeField] private GameObject emptyCanvas;

        private float segmentAngle;

        private void Awake() {
            segmentAngle = 360 / items.Length;
            SetupItems();
            SetupSelectedSegment();
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        private void SetupItems() {
            for (int i = 0; i < items.Length; i++) {
                float angle = i * segmentAngle;
                InstantiateItemCanvas(items[i].Sprite, angle);
            }
        }

        private void SetupSelectedSegment() {

        }

        private void InstantiateItemCanvas(Sprite image, float angle) {
            // instantiate image and helper
            GameObject midPoint = new();
            midPoint.transform.parent = transform;
            midPoint.transform.position = transform.position;
            GameObject segment = Instantiate(emptyCanvas);
            segment.transform.parent = midPoint.transform;

            // mutate item
            segment.GetComponent<Image>().sprite = image;
            RectTransform rect = segment.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            rect.localPosition = new Vector3(0, 3.5f, 0);
            midPoint.transform.eulerAngles = new Vector3(90, 0, angle);

            // destroy helperParent
            segment.transform.parent = transform;
            rect.localEulerAngles = new Vector3(0, 0, 0);
            Destroy(midPoint);
        }
    }
}