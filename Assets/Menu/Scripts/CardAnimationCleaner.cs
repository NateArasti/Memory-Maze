using UnityEngine;
using UnityEngine.UI;

public class CardAnimationCleaner : MonoBehaviour
{
    [SerializeField] private Image[] background;
    [SerializeField] private Text[] text;
    [SerializeField] private Text[] description;
    
    public void ClearAnimation()
    {
        for (var i = 0; i < background.Length; i++)
        {
            background[i].color = new Color(0.73725f, 0.73725f, 0.73725f);
            text[i].color = Color.white;
            text[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);
            description[i].GetComponent<Text>().color = Color.white;
            description[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
        }
    }
}
