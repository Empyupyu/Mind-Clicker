using UnityEngine;

public class SqrtPristigeCalculate : IPristigeCalculate
{
    public int Calculate(float value)
    {
       return Mathf.FloorToInt(Mathf.Sqrt(value));
    }
}