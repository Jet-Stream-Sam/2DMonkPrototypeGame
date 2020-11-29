
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Player Attack")]
public class PlayerAttack : Attack
{
    [PropertyOrder(-10)]
    [Title("PlayerAttack", TitleAlignment = TitleAlignments.Centered, Bold = true)]
    [PropertySpace]

    [Title("Attack Behaviour")]
    [HideLabel]
    [RequireInterface(typeof(IAttackBehaviour))]
    public Object attackBehaviour;

    public enum EndsAtState
    {
        Standing,
        Crouching,
        Falling
    }

    public EndsAtState attackEndsAtState;

    [Title("Attack Notation")]
    

    

    [HideLabel]
    public AttackNotation attackNotation;

}
