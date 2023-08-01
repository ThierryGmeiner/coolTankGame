using UnityEngine;
using UnityEngine.UI;
using Game.Data;

namespace Game.UI
{
    public class ScrolingWheelSegment
    {
        public Item Item { get; }
        public Image image { get; }
        public RectTransform rect{ get; }

        public ScrolingWheelSegment(Item item, GameObject obj) {
            Item = item;
            image = obj.GetComponent<Image>() ?? obj.AddComponent<Image>();
            rect = obj.GetComponent<RectTransform>() ?? obj.AddComponent<RectTransform>();
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