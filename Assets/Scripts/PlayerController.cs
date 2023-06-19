using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D _rigidbody;
    public float hp;

    private FixedJoystick _joystick;
    private OxygenController _oxygenController;
    private Attack _attack;
    private bool _stunStatus = false;
    private float _stunDuration = 5f;
    private float _speed = 10f;
    private Vector2 _movement;
    private static readonly int HasOxygen = Animator.StringToHash("hasOxygen");
    private static readonly int XDirection = Animator.StringToHash("xDirection");
    private static readonly int YDirection = Animator.StringToHash("yDirection");
    private Vector2 _lastMovement;
    [SerializeField]
    private Animator playerAnimator;

    void Start()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _attack = gameObject.GetComponent<Attack>();
    }

    void FixedUpdate()
    {
        //Todo delete when game will be done, it's just for dev
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            if (!IsOwner)
            {
                return;
            }
        }
        
        _movement = new Vector2(_joystick.Horizontal, _joystick.Vertical).normalized;
        if (_movement != Vector2.zero) {
            _lastMovement = _movement;
            _rigidbody.AddForce(_movement * _speed, ForceMode2D.Force);
        } else {
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, Time.deltaTime * 1);
        }
    
        playerAnimator.SetFloat(XDirection, _movement.x);
        playerAnimator.SetFloat(YDirection, _movement.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Borders")
        {
            Vector2 reflectedDirection = Vector2.Reflect(_lastMovement.normalized, collision.contacts[0].normal);
            _rigidbody.AddForce(reflectedDirection * _speed, ForceMode2D.Impulse);
        }
    }
    
    public void OnAttackButton()
    {
        if (_stunStatus != true)
        {
            _attack.Attacks();
        }
    }

    public void TakeDamage(float damage)
    {
        if (hp > 0f)
        {
            hp -= damage;
        }
        if (hp <= 0f)
        {
            _oxygenController = transform.Find("Oxygen").gameObject.GetComponent<OxygenController>();
            _oxygenController.LoseObject();
            _speed = 0f;
            _stunStatus = true;
            GetComponent<Animator>().SetBool(HasOxygen, false);
            StartCoroutine(DisableStunAfterDelay(_stunDuration));
        }   
    }
    
    private IEnumerator DisableStunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableStun();
    }


    private void DisableStun()
    {
      _speed = 10f;
      _stunStatus = false;
      hp = 100;
      GameObject hpBar = transform.Find("HpStats").gameObject;
      hpBar.SetActive(false);
    }
}