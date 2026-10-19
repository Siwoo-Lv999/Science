using TMPro;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    public CurrentModel currentModel;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI directionText;

    void Awake()
    {
        strengthText.text = $"{currentModel.Strength}";
        directionText.text = currentModel.IsLeft ? "N" : "S";
        
        currentModel.OnChanged += () =>
        {
            strengthText.text = $"{currentModel.Strength}";
            directionText.text = currentModel.IsLeft ? "N" : "S";
        };
    }
    
    


   
}
