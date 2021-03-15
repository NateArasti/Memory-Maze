using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("Set in Inspector")] public Vector3 translation;
    public float speed = 2;
    public GameObject background;

    private float _spriteWidth;
    private GameObject _firstPiece;
    private GameObject _secondPiece;
    private int _movement;

    private void Start()
    {
        _movement = Random.Range(0, 2);
        translation = _movement == 0 ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
        SpawnBackground();
    }

    private void Update()
    {
        Movement();
    }

    private void SpawnBackground()
    {
        _spriteWidth = background.GetComponent<SpriteRenderer>().sprite.rect.width / 100;
        _firstPiece = Instantiate(background, new Vector2(0, 0), Quaternion.identity);
        _secondPiece = Instantiate(background, new Vector2(translation.x * _spriteWidth, 0), Quaternion.identity);
    }

    private void Movement()
    {
        _firstPiece.transform.Translate(translation * (speed * Time.deltaTime));
        _secondPiece.transform.Translate(translation * (speed * Time.deltaTime));
        if (translation.x * _firstPiece.transform.position.x > _spriteWidth)
            _firstPiece.transform.position =
                _secondPiece.transform.position - translation.x * new Vector3(_spriteWidth, 0);

        if (translation.x * _secondPiece.transform.position.x > _spriteWidth)
            _secondPiece.transform.position =
                _firstPiece.transform.position - translation.x * new Vector3(_spriteWidth, 0);

        _firstPiece.transform.position = new Vector3(_firstPiece.transform.position.x, Mathf.Sin(Time.time) / 2);
        _secondPiece.transform.position = new Vector3(_secondPiece.transform.position.x, Mathf.Sin(Time.time) / 2);
    }
}