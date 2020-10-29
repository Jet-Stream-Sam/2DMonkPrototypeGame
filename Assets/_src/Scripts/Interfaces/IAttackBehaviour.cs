
public interface IAttackBehaviour
{
    void Init(PlayerMainController controllerScript);
    void OnAttackEnter();

    void OnAttackUpdate();

    void OnAttackFixedUpdate();

    void OnAttackExit();
}
