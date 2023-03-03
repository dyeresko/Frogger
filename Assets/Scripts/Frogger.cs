using UnityEngine;
using System.Collections;

public class Frogger : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public Sprite leapSprite;
    public Sprite idleSprite;
    public Sprite deadSprite;
    Vector3 spawnPosition;

    private float farthestRow;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = this.transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            Move(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            Move(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {

        Vector3 destination = this.transform.position + direction;
        Collider2D barrier = Physics2D.OverlapBox(destination,
        Vector2.zero, 0.0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination,
        Vector2.zero, 0.0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination,
        Vector2.zero, 0.0f, LayerMask.GetMask("Obstacle"));
        if (barrier != null)
        {
            return;
        }
        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        if (platform == null && obstacle != null)
        {
            transform.position = destination;
            Death();
        }
        else
        {
            if (destination.y > farthestRow)
            {
                farthestRow = destination.y;
                FindObjectOfType<GameManager>().AdvancedRow();
            }
            StartCoroutine(Leap(destination));
        }
    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startingPosition = this.transform.position;
        float elapsed = 0.0f;
        float duration = 0.125f;
        this.spriteRenderer.sprite = leapSprite;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startingPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.spriteRenderer.sprite = idleSprite;

        this.transform.position = destination;

    }

    public void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        enabled = false;
        FindObjectOfType<GameManager>().Died();
    }

    public void Respawn()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = idleSprite;
        farthestRow = spawnPosition.y;
        enabled = true;
        this.transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();
        }
    }
}
