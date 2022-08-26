using System.Collections.Generic;
using UnityEngine;

public class Sneky : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public static bool dead = false;
    public Transform segmentPrefab;
    public int initialSize = 3;

    public static int score = 0;

    public SpriteRenderer spriteRenderer;
    public Sprite headFacingUp;
    public Sprite headFacingDown;
    public Sprite headFacingLeft;
    public Sprite headFacingRight;
    public Sprite deadSnakeWalking;

    public AudioSource audioSource;
    public AudioClip[] audioClips;
    /* audio clip guide:
     * size: 5+
     * element 0: up sound
     * element 1: down sound
     * element 2: left sound
     * element 3: right sound
     * element 4: eat sound
     */

    private void Start() {ResetState();}

    private void Update()
    {
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down && _direction != Vector2.up)
            {
                _direction = Vector2.up;
                audioSource.PlayOneShot(audioClips[0]);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up && _direction != Vector2.down)
            {
                _direction = Vector2.down;
                audioSource.PlayOneShot(audioClips[1]);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right && _direction != Vector2.left)
            {
                _direction = Vector2.left;
                audioSource.PlayOneShot(audioClips[2]);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left && _direction != Vector2.right)
            {
                _direction = Vector2.right;
                audioSource.PlayOneShot(audioClips[3]);
            }
            if (_direction == Vector2.up) { spriteRenderer.sprite = headFacingUp; }
            else if (_direction == Vector2.down) { spriteRenderer.sprite = headFacingDown; }
            else if (_direction == Vector2.left) { spriteRenderer.sprite = headFacingLeft; }
            else if (_direction == Vector2.right) { spriteRenderer.sprite = headFacingRight; }
        }
        else {_direction = Vector2.zero;}
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--) {_segments[i].position = _segments[i - 1].position;}

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
            );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);

        score++;
    }

    public void ResetState()
    {
        dead = false;
        for (int i = 1; i < _segments.Count; i++) {Destroy(_segments[i].gameObject);}
        
        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector3.zero;
        for (int i = 1; i < this.initialSize; i++) {_segments.Add(Instantiate(this.segmentPrefab));}
        _direction = Vector2.right;
        score = 0;

        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple")
        {
            audioSource.PlayOneShot(audioClips[4]);
            Grow();
        }
        else if (other.tag == "D e a t h")
        {
            audioSource.Stop();
            spriteRenderer.sprite = deadSnakeWalking;
            dead = true;
        }
    }
}
