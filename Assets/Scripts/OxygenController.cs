using Unity.Netcode;
using UnityEngine;

public class OxygenController : NetworkBehaviour
{
    [SerializeField] public float speed = 2f;
    [SerializeField] public float immortalTime = 3f;
    
    private Transform _transform;
    private Rigidbody2D _rb2d;
    private Vector2 _direction;
    private static readonly int HasOxygen = Animator.StringToHash("hasOxygen");

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _direction = Random.insideUnitCircle.normalized;
    }

    private void FixedUpdate()
    {
        if (_rb2d.bodyType == RigidbodyType2D.Dynamic) {
            _rb2d.velocity = _direction * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Borders") {
            _direction = Vector2.Reflect(_direction, collision.contacts[0].normal);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            _transform.SetParent(player.transform);
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            _transform.localPosition = new Vector2(0f, 0f);
            GetComponent<Collider2D>().enabled = false;
            player.gameObject.tag = "PlayerOxy";
            player.transform.Find("HpStats")?.gameObject.SetActive(true);
            player.GetComponent<Animator>().SetBool(HasOxygen, true);
            gameObject.SetActive(false);
        }
    }
    public void LoseObject()
    {
        gameObject.SetActive(true);
        _transform.parent.tag = "Player";
        _transform.parent = null;
        Collider2D[] colliders = GetComponents<Collider2D>();
        colliders[1].enabled = false;
        colliders[0].enabled = true;
        colliders[0].gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
        _direction = Random.insideUnitCircle.normalized;
        Invoke(nameof(ActivateTrigger), immortalTime);
    }
    
    private void ActivateTrigger()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        colliders[1].enabled = true;
        colliders[0].gameObject.layer = LayerMask.NameToLayer("Default");
    }
}