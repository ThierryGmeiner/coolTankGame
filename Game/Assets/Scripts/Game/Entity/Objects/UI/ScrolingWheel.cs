using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public class ScrolingWheel : MonoBehaviour
    {
        private RectTransform rectTransform;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}