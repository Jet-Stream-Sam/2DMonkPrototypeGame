
public interface IMoveBehaviour
{
    void Init(PlayerMainController controllerScript, PlayerMoves attackAsset);
    void OnMoveEnter();

    void OnMoveUpdate();

    void OnMoveFixedUpdate();

    void OnMoveExit();
}
