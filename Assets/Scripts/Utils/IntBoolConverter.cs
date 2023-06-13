using UnityEngine;

namespace Utilities {
    public static class IntBoolConverter {
        public static string saveFilePath = Application.persistentDataPath + "/preferences.file";

        public static bool IntToBool(int value) {
            if (value == 0) return false;
            else return true;
        }

        public static int BoolToInt(bool value) {
            if (value)
                return 1;
            else return 0;
        }
    }
}