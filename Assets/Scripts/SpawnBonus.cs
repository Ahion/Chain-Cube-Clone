using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBonus : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject bonusPrefab;

    private Button button;
    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateBonus);
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }
    public void CreateBonus()
    {
        //Debug.Log(gameObject.name + " was clicked");
        gameManagerScript.SpawnBonus(bonusPrefab);
    }
}
