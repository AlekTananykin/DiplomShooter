public interface IPlayFabObserver
{
    void OnLoginPlayFabSuccess(string message);
    void OnLoginPlayFabFail(string message);
    void OnCreateAccountPlayFabSuccess(string result);
    void OnCreateAccountPlayFabFail(string error);
    void OnUpdateUserData(int killes, int killed);
}