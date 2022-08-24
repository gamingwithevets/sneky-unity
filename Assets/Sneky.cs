using System.Collections.Generic;
using UnityEngine;

public class Sneky : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    public int initialSize = 3;

    private void Start() {ResetState();}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down) {_direction = Vector2.up;}
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up) {_direction = Vector2.down;}
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right) {_direction = Vector2.left;}
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left) {_direction = Vector2.right;}
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
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++) {Destroy(_segments[i].gameObject);}
        
        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector3.zero;
        for (int i = 1; i < this.initialSize; i++) {_segments.Add(Instantiate(this.segmentPrefab));}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple") {Grow();}
        else if (other.tag == "D e a t h") {ResetState();}
    }
}
