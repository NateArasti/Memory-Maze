using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector3 translation;
    public float speed = 2;
    public GameObject background;

    private float spriteWidth;
    private GameObject firstPiece;
    private GameObject secondPiece;
    private int movement;

    void Start()
    {
        movement = Random.Range(0, 2);
        if (movement == 0)
        {
            translation = new Vector3(-1, 0, 0);
        }
        else
        {
            translation = new Vector3(1, 0, 0);
        }
        SpawnBackground();
    }

    void Update()
    {
        Movement();
    }

    public void SpawnBackground()
    {
        spriteWidth = background.GetComponent<SpriteRenderer>().sprite.rect.width / 100;
        firstPiece = Instantiate(background, new Vector2(0, 0), Quaternion.identity);
        secondPiece = Instantiate(background, new Vector2(translation.x * spriteWidth, 0), Quaternion.identity);
    }

    private void Movement()
    {
        firstPiece.transform.Translate(translation * speed * Time.deltaTime);
        secondPiece.transform.Translate(translation * speed * Time.deltaTime);
        if (translation.x * firstPiece.transform.position.x > spriteWidth)
        {
            firstPiece.transform.position =
                secondPiece.transform.position - translation.x * new Vector3(spriteWidth, 0);
        }
        if (translation.x * secondPiece.transform.position.x > spriteWidth)
        {
            secondPiece.transform.position =
                firstPiece.transform.position - translation.x * new Vector3(spriteWidth, 0);
        }
        firstPiece.transform.position = new Vector3(firstPiece.transform.position.x, Mathf.Sin(Time.time) / 2);
        secondPiece.transform.position = new Vector3(secondPiece.transform.position.x, Mathf.Sin(Time.time) / 2);
    }
}
