using UnityEngine;

[CreateAssetMenu(fileName = "ModulePrioritiesConfig", menuName = "Create Game Datas/ModulePrioritiesConfig")]
public class ModulePrioritiesConfig : ScriptableObject
{
    public int Authorization = 0;
    public int SaveLoad = 10;
    public int Advertisement = 20;
    public int AdvertisementReward = 21;
    public int Analytics = 30;
}
