namespace Magic
{
    public static class Array
    {
        public static bool Contains<T>(T[] array, T obj) {
            foreach(T n in array) {
                if (obj.Equals(n)) {
                    return true;
                }
            }
            return false;
        }
    
        public static bool Contains<T>(T[,] array, T obj) {
            foreach(T n in array) {
                if (obj.Equals(n)) {
                    return true;
                }
            }
            return false;
        }
    }
}