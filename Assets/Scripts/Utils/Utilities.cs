using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class Utilities
{

    public static string saveFilePath= Application.persistentDataPath + "/preferences.file";

    public static bool IntToBool(int value) {
        if(value==0)return false;
        else return true;
    }

    public static int BoolToInt(bool value) {
        if (value)
            return 1;
        else return 0;
    }
}
