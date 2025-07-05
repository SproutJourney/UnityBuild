using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public GameObject selectedSkin; 
    public GameObject Player;       

    private Sprite playerSprite;

    void Start()
    {

        if (selectedSkin != null)
        {
            playerSprite = selectedSkin.GetComponent<SpriteRenderer>().sprite;
        }


        if (Player == null)
        {
            Player = Instantiate(Resources.Load("PlayerPrefab") as GameObject);
            Player.transform.position = new Vector3(-6.19f, 0.899f, 0f);
        }

        var playerRenderer = Player.GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            playerRenderer.sprite = playerSprite;
        }
    }
}
