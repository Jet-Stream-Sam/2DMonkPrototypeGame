using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Player Attack")]
public class PlayerAttack : Attack
{
   public enum EndsAtState
   {
       Standing,
       Crouching,
       Falling
   }

    public EndsAtState attackEndsAtState;

    [RequireInterface(typeof(IAttackBehaviour))]
    public Object attackBehaviour;
    
    
}
