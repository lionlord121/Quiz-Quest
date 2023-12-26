using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CategoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject categoryButton;
    [SerializeField]
    private Transform categoryPanel;
    public void UpdateCategories(List<string> categories)
    {
        foreach (string category in categories)
        {
            Transform categoryButtonInstance = Instantiate(categoryButton, categoryPanel).transform;
            categoryButtonInstance.GetComponent<CategoryButton>().SetCategoryButton(category);
        }
    }
}
