using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{   public static GameController Instance { get; private set; }
    public player Player { get; private set; }
    public UIController UIController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        Player = playerObj.GetComponent<player>();
        GameObject UIControllerObj = GameObject.FindWithTag("UIController");
        UIController = UIControllerObj.GetComponent<UIController>();

        Instance.Player.HandSelected += PlayerHandSelected;
        Instance.Player.InventoryUpdated += PlayerInventoryUpdated;

    }

    void PlayerHandSelected(string hand)
    {
        if (hand == "left")
        {
            UIController.select_left_hand();
            UIController.unselect_right_hand();
        }
        if (hand == "right")
        {
            UIController.select_right_hand();
            UIController.unselect_left_hand();
        }
    }

    void PlayerInventoryUpdated(List<Interactable> inventory)
    {
        UIController.update_hotbar_display(inventory);
    }


}
