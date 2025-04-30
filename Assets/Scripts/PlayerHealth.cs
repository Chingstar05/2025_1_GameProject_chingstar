using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int CurrentLives;

    public float invincibleTime = 1.0f;
    public bool islnvicible = false;

    // Start is called before the first frame update
    void Start()
    {
        CurrentLives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {




    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Miscile"))
        {
            CurrentLives--;
            Destroy(other.gameObject);

            if (CurrentLives == 0)
            {
                GameOver();
            }

        }

    }
    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestartGame", 3.0f);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }













}





