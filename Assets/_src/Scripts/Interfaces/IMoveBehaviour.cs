
public interface IMoveBehaviour
{
    void Init(IEntityController controllerScript, Moves attackAsset, IState state);
    void OnMoveEnter();

    void OnMoveUpdate();

    void OnMoveFixedUpdate();

    void OnMoveExit();
}
