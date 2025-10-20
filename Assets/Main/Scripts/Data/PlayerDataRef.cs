public class PlayerDataRef
{
    public PlayerData Value { get; private set; }

    public void Set(PlayerData newData)
    {
        Value = newData;
    }
}