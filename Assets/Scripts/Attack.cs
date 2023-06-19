using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Attack : NetworkBehaviour
{
    public bool isButtonPressed = false;
    [SerializeField]
	private float delayTime = 0.1f;
	public PlayerController attackedPlayerController;
	private OxygenController _oxygenController;
	[SerializeField]
	private float attackDamage = 34;

	public void Attacks()
    {
	    if (!IsOwner)
	    {
		    return;
	    }
        isButtonPressed = true;
        Invoke(nameof(FinishAttack), delayTime);
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
	    if (!isButtonPressed || !collider2D.CompareTag("PlayerOxy")) return;
	    attackedPlayerController = collider2D.GetComponent<PlayerController>();
	    attackedPlayerController.TakeDamage(attackDamage);
	    FinishAttack();
    }

	private void FinishAttack() 
	{
		isButtonPressed = false;
	}
}
