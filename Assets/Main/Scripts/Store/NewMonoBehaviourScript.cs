using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Product
{
    public string name;
    public float basePrice; // сколько он реально стоит
    public float currentPrice; // цена, которую установил игрок
}