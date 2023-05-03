using UnityEngine;


[CreateAssetMenu(fileName ="PlayerController",menuName ="InputController/PlayerController")]
public class PlayerController : InputController
{

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveDodgeInput()
    {
        return Input.GetButtonDown("Roll");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Attack1");
    }
}
