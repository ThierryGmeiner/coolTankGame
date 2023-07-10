using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public static class SceneHelper
    {
        public static T[] GetObjectsFromRoot<T>(GameObject rootObject) {
            if (rootObject == null) { return new T[0]; }

            List<T> list = new List<T>();
            for (int i = 0; i < rootObject.transform.childCount; i++) {
                T obj = rootObject.transform.GetChild(i).GetComponent<T>();
                if (obj != null) { list.Add(obj); }
            }
            return list.ToArray();
        }

        public static T[] GetObjectsFromRoot<T>(GameObject rootObject, T exeption) {
            if (rootObject == null) { return new T[0]; }

            List<T> list = new List<T>();
            for (int i = 0; i < rootObject.transform.childCount; i++) {
                T obj = rootObject.transform.GetChild(i).GetComponent<T>();
                if (obj != null && !obj.Equals(exeption)) { list.Add(obj); }
            }
            return list.ToArray();
        }

    }
}