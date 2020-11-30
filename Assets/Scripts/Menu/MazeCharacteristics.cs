using UnityEngine;
using UnityEngine.UI;

public class MazeCharacteristics : MonoBehaviour
{
    public int valueDelta = 1;
    public InputField ValueToChange;

    public void Awake()
    {
        if (name != "Width" && name != "Height" && name!= "Side")
            return;
        if (!PlayerPrefs.HasKey(name))
           PlayerPrefs.SetInt(name, 5);
        ValueToChange.text = PlayerPrefs.GetInt(name).ToString();
    }

    public void ChangeValue()
    {
        ValueToChange.text = (int.Parse(ValueToChange.text) + valueDelta).ToString();
        CheckValue();
    }
    public void CheckValue()
    {
        int value;
        try
        {
            value = int.Parse(ValueToChange.text);
        }
        catch
        {
            value = 0;
        }
        if (value < 5)
            ValueToChange.text = "5";
        else if (value > 40)
            ValueToChange.text = "40";
        PlayerPrefs.SetInt(ValueToChange.name, int.Parse(ValueToChange.text));
    }
}
