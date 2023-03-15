
public interface ISoljaGameManager
{
    void SetHealth(int health);
    void SetKills(int killsAccount);

    void OnUpdateHealth(int health);

    void IncreaseKillCounter(int killsAccount);
    void GiveUp();

    void CheckPlayerWine();
}
