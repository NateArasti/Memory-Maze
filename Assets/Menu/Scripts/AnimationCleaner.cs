using UnityEngine;
using UnityEngine.UI;

public class AnimationCleaner : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Text text;
    [SerializeField] private Text description;
    
    public void ClearAnimation()
    {
        background.color = new Color(0.73725f, 0.73725f, 0.73725f);
        text.color = Color.white;
        text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);
        description.GetComponent<Text>().color = Color.white;
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
    }
}
