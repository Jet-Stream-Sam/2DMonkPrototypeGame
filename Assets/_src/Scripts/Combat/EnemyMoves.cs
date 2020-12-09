using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Enemy Moves")]
public class EnemyMoves : Moves
{
    [PropertyOrder(-10)]
    [Title("EnemyMoves", TitleAlignment = TitleAlignments.Centered, Bold = true)]
    [PropertySpace]

    [Title("Move Behaviour")]
    [HideLabel]
    [RequireInterface(typeof(IMoveBehaviour))]
    public Object moveBehaviour;
}
