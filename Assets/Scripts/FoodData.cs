using UnityEngine;

public enum foodType
{
    Cheese,
    Pickle,
    Potatoe,
    Ham
}

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]

public class FoodData : ScriptableObject
{
    public string foodName;
    public foodType foodType;

    public int startingScore;
    public int minimumScore;
    public int maximumScore;
}
