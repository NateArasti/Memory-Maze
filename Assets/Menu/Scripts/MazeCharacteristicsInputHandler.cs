using UnityEngine;
using UnityEngine.UI;

public class MazeCharacteristicsInputHandler : MonoBehaviour
{
    public int valueDelta = 1;
    public InputField valueToChange;

    public void Awake()
    {
        if (name != "Width" && name != "Height" && name != "Side Size")
            return;
        if (!PlayerPrefs.HasKey(name))
           PlayerPrefs.SetInt(name, 5);
        valueToChange.text = PlayerPrefs.GetInt(name).ToString();
    }

    public void ChangeValue()
    {
        valueToChange.text = (int.Parse(valueToChange.text) + valueDelta).ToString();
        CheckValue();
    }
    
    public void CheckValue()
    {
        var result = int.TryParse(valueToChange.text, out var value);
        value = result ? value : 0;
        if (value < 5)
            valueToChange.text = "5";
        else if (value > 40)
            valueToChange.text = "40";
        PlayerPrefs.SetInt(valueToChange.name, int.Parse(valueToChange.text));
    }
}
