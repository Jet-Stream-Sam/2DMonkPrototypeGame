
public interface IAttackBehaviour
{
    void Init(PlayerMainController controllerScript, PlayerAttack attackAsset);
    void OnAttackEnter();

    void OnAttackUpdate();

    void OnAttackFixedUpdate();

    void OnAttackExit();
}
