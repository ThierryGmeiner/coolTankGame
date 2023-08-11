using UnityEngine;
using Game.UI;
using Game.Data;

namespace Game.Entity
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private ScrolingWheel scrolingWheel;

        private void Start() {
            scrolingWheel.OnSelectItem += SelectItem;
        }

        private void SelectItem(Item item) {

            Debug.Log(item.typeID);
            if (IsBulletID(item.typeID)) {
                Debug.Log("isBullet");
            }
            else if (IsMineID(item.typeID)) {
                Debug.Log("isMine");
            }
            else if (IsBuildableID(item.typeID)) {
                Debug.Log("isBuildable");
            }
        }

        private void GetObject(byte id) {
            System.Convert.ToString(id, toBase: 2);
        }

        private bool IsBulletID(string id) => id == "0";
        private bool IsMineID(string id) => id == "1";
        private bool IsBuildableID(string id) => id == "10";
    }
}