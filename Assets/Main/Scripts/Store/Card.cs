using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private Product product;

    public Product GetProduct() { return product; }
    
    public void ChangePrice(float price)
    {
        product.currentPrice = price;
    }
}
