using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]

public class AIController : InputController
{
    public override bool RetrieveAttackInput()
    {
        return true;
    }

    public override bool RetrieveDodgeInput()
    {
        return true;
    }

    public override bool RetrieveJumpInput()
    {
        return true;
    }

    public override float RetrieveMoveInput()
    {
        return 1f;
    }

}
