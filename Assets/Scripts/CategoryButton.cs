using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryButton : MonoBehaviour
{
    private Game game;
    private string category;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => game.CategorySelected(category));
    }
    public void SetCategoryButton(string category)
    {
        this.category = category;
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = category;
    }

    public string GetCategory()
    {
        return this.category;
    }
}
