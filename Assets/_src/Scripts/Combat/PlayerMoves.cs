
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Player Moves")]
public class PlayerMoves : Moves
{
    [PropertyOrder(-10)]
    [Title("PlayerMoves", TitleAlignment = TitleAlignments.Centered, Bold = true)]
    [PropertySpace]

    [Title("Move Behaviour")]
    [HideLabel]
    [RequireInterface(typeof(IMoveBehaviour))]
    public Object moveBehaviour;

    public enum EndsAtState
    {
        Standing,
        Crouching,
        Falling
    }
    [PropertyOrder(-2)]
    public EndsAtState moveEndsAtState;

    [PropertyOrder(-2)]
    public int powerMeterUsage = 0;
    [PropertyOrder(-2)]
    public int vitalityMeterUsage = 0;

    [Title("Move Notation")]

    [HideLabel]
    public MoveNotation moveNotation;

    private void OnValidate()
    {
        moveNotation.NotationCheck();
    }
}
