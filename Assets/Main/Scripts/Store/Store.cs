using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [Header("Товары")]
    public List<Card> products;

    [Header("Интервал выбора товара (секунды)")]
    public float minInterval = 5f;
    public float maxInterval = 15f;

    [Header("Время на размышление перед покупкой")]
    public float decisionDelay = 3f;

    private Card currentProduct;
    private Card card;

    void Start()
    {
        StartCoroutine(ProductLoop());
    }

    public void SelectCard(Card card)
    {
        this.card = card;
    }

    public Card GetCard()
    {
        return this.card;
    }

    IEnumerator ProductLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            currentProduct = products[Random.Range(0, products.Count)];
            Debug.Log($"Покупатель смотрит на товар: {currentProduct.name} по цене {currentProduct.GetProduct().currentPrice}");

            yield return new WaitForSeconds(decisionDelay);
            EvaluateProduct(currentProduct.GetProduct());
        }
    }

    void EvaluateProduct(Product product)
    {
        float ratio = product.currentPrice / product.basePrice;
        string reaction;
        string emoji;

        if (ratio > 1.5f)
        {
            reaction = "Слишком дорого. Отказ.";
            emoji = "😢";
        }
        else if (ratio > 1.1f)
        {
            reaction = "Дороговато, но можно купить.";
            emoji = "😐";
        }
        else if (ratio > 0.8f)
        {
            reaction = "Хорошая цена. Покупаю!";
            emoji = "🙂";
        }
        else
        {
            reaction = "Ого, скидка! Беру!";
            emoji = "🤩";
        }

        Debug.Log($"Реакция на {product.name}: {reaction} {emoji}");
    }

    // Метод для изменения цены игроком
    public void SetProductPrice(string productName, float newPrice)
    {
        // Product p = products.Find(x => x.name == productName);
        // if (p != null)
        // {
        //     p.currentPrice = newPrice;
        //     Debug.Log($"Цена на {productName} изменена игроком: {newPrice}");
        // }
        // else
        // {
        //     Debug.LogWarning($"Товар {productName} не найден.");
        // }
    }
}