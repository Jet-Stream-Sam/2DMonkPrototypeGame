using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Player Attack")]
public class PlayerAttack : Attack
{
    [Header("Attack Behaviour")]
    [RequireInterface(typeof(IAttackBehaviour))]
    public Object attackBehaviour;

    public enum EndsAtState
    {
        Standing,
        Crouching,
        Falling
    }

    public EndsAtState attackEndsAtState;

}
