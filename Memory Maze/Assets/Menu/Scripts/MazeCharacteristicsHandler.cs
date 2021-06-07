using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeCharacteristicsHandler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private MazeType mazeType;
    [SerializeField] private TMP_InputField[] characteristics;
    private int valueDelta = 1;

    public void Start()
    {
        for(var i = 0; i < characteristics.Length; ++i)
            characteristics[i].text = MazeCharacteristics.Characteristics[mazeType].paramValues[i].ToString();
    }

    public void ChangeValue(int characteristicIndex)
    {
        characteristics[characteristicIndex].text =
            (int.Parse(characteristics[characteristicIndex].text) + valueDelta).ToString();
        CheckValue(characteristicIndex);
    }

    public void SetValueDelta(int value) => valueDelta = value;

    public void CheckValue(int characteristicIndex)
    {
        var result = int.TryParse(characteristics[characteristicIndex].text, out var value);
        value = result ? value : 0;
        var valueRange = MazeCharacteristics.Characteristics[mazeType].valueRange;
        if (value < valueRange.x)
            characteristics[characteristicIndex].text = valueRange.x.ToString(CultureInfo.InvariantCulture);
        else if (value > valueRange.y)
            characteristics[characteristicIndex].text = valueRange.y.ToString(CultureInfo.InvariantCulture);
    }

    public void SaveCharacteristics()
    {
        MazeCharacteristics.SetMazeCharacteristics(new MazeData(mazeType,
            characteristics.Select(input => int.Parse(input.text)).ToArray()));
        MazeCharacteristics.SaveMazesCharacteristics();
    }
}