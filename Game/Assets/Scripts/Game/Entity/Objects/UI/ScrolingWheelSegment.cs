using UnityEngine;
using UnityEngine.UI;
using Game.Data;

namespace Game.UI
{
    public class ScrolingWheelSegment
    {
        public Item item { get; }
        public Image image { get; }
        public RectTransform rect{ get; }

        private static readonly Vector3 DEFAULT_SCALE = new Vector3(0.15f, 0.15f, 0.15f);
        private static readonly Vector3 SELECTED_SCALE = new Vector3(0.23f, 0.23f, 0.23f);

        public ScrolingWheelSegment(Item item, GameObject obj) {
            this.item = item;
            image = obj.GetComponent<Image>() ?? obj.AddComponent<Image>();
            rect = obj.GetComponent<RectTransform>() ?? obj.AddComponent<RectTransform>();
        }

        public void SelectItem() {
            image.sprite = item.SpriteWhenSelected;
            rect.localScale = SELECTED_SCALE;
        }

        public void DeselectItem() {
            image.sprite = item.Sprite;
            rect.localScale = DEFAULT_SCALE;
        }

        public static GameObject InstantiateItemCanvas(Transform parent, float angle) {
            // instantiate image and helper
            GameObject midPoint = new GameObject();
            GameObject segment = ScrolingWheelSegment.EmptyCanvas();
            midPoint.transform.SetParent(parent);
            midPoint.transform.position = parent.position;
            segment.transform.SetParent(midPoint.transform);

            // mutate item
            RectTransform rect = segment.GetComponent<RectTransform>();
            rect.localScale = DEFAULT_SCALE;
            rect.localPosition = new Vector3(0, 3.5f, 0);
            midPoint.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            segment.transform.SetParent(parent);
            rect.localRotation = Quaternion.Euler(0, 0, 0);
            GameObject.Destroy(midPoint);

            return segment;
        }

        public static GameObject EmptyCanvas() {
            GameObject canvas = new GameObject();
            RectTransform rect = canvas.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(10, 10);
            canvas.AddComponent<CanvasRenderer>();
            canvas.AddComponent<Image>();
            return canvas;
        }
    }
}