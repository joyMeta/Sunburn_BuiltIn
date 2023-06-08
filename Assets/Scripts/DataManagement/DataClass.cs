
[System.Serializable]
public class DataClass
{
    public int index;
    public string question;
    public string answer;
}

[System.Serializable]
public class DataList {
    public DataClass[] data;
}