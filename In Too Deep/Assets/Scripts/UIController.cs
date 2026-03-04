using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private Slider _healthbar;
    [SerializeField] private Slider _lazybar;
    [SerializeField] private Slider _chargebar;
    [SerializeField] private Image _chargebarfill;
    [SerializeField] private float _healingrate;
    [SerializeField] private float _damagerate;
    [SerializeField] private float _maximum_height;
    [SerializeField] private TextMeshProUGUI _depth_text;
    [SerializeField] private TextMeshProUGUI _questStatusText;
    [SerializeField] private Image _right_hand;
    [SerializeField] private Image _left_hand;
    [SerializeField] private Sprite _left_hand_unselected;
    [SerializeField] private Sprite _left_hand_selected;
    [SerializeField] private Sprite _right_hand_unselected;
    [SerializeField] private Sprite _right_hand_selected;

    [SerializeField] private Image _right_hand_item;
    [SerializeField] private Image _left_hand_item;
    [SerializeField] private Sprite _cheese;
    [SerializeField] private Sprite _cheese_wheel;
    [SerializeField] private Sprite _pie;
    [SerializeField] private Sprite _stew;
    private Transform _respawnPoint;

    private List<Image> _item_display_list = new List<Image>();


    public string _currentQuestStatus;
    private float seconds_of_healing=0;
    private float seconds_of_damage = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _currentQuestStatus = "None";
        _item_display_list.Add(_left_hand_item);
        _item_display_list.Add(_right_hand_item);
        losehealth(5);
    }

    
    // Update is called once per frame
    void Update()
    {
        player player = GameController.Instance.Player;
        _depth_text.text = "Depth: " + Mathf.Round(1000 * (1- (GameController.Instance.Player.transform.position.y / _maximum_height))).ToString() + " M";

        if (_lazybar.value > _healthbar.value)
        {
            _lazybar.value -= _damagerate * Time.deltaTime;
        }

        if (seconds_of_healing > 0)
        {
            seconds_of_healing -= Time.deltaTime;
            player._health += Time.deltaTime * _healingrate;
            if (player._health > player._maxHealth)
            {
                player._health = player._maxHealth;
            }

            _healthbar.value += Time.deltaTime * _healingrate;

        }

        if (seconds_of_damage > 0)
        {
            seconds_of_damage -= Time.deltaTime;
            player._health -= Time.deltaTime * _damagerate;
        }

        if (player._health <= 0)
        {
            player._health = player._maxHealth;
            _healthbar.value = player._health;
            Respawn();
        }


        if (player._charging == true)
        {
            _chargebar.value = player._charge_percent * 200;
            _chargebarfill.color = new Color( (38 + player._charge_percent * (255 -38)) / 255, (255 - player._charge_percent * (255 - 38)) / 255, (59 - player._charge_percent * (59-38)) / 255, 1);
            //_chargebarfill.color = new Color(255,                             38, 38, 1);

        }
        else
        {
            _chargebar.value = 0;
        }

        /////////////// _questStatusText.text = "Quest Status: " + _currentQuestStatus;

    }


    public void losehealth(float damage_seconds)
    {
        if (_lazybar.value < _healthbar.value)
        {
            _lazybar.value = _healthbar.value;
        }

        _healthbar.value = _healthbar.value -= damage_seconds * _damagerate;
        seconds_of_damage += damage_seconds;
    }

    public void gainhealth(float healing_seconds)
    {
        seconds_of_healing += healing_seconds;
    }

    public void select_right_hand()
    {
        _right_hand.sprite = _right_hand_selected;
    }
    public void select_left_hand()
    {
        _left_hand.sprite = _left_hand_selected;
    }
    public void unselect_right_hand()
    {
        _right_hand.sprite = _right_hand_unselected;
    }

    public void unselect_left_hand()
    {
        _left_hand.sprite = _left_hand_unselected;
    }

    public void update_hotbar_display(List<Interactable> inventory)
    {
        foreach (int i in Enumerable.Range(0, 2))
        {
            if (inventory[i].type!="None")
            {
                _item_display_list[i].gameObject.SetActive(true);
                switch (inventory[i].item_name)
                {
                    case "Cheese":
                        _item_display_list[i].sprite = _cheese;
                        break;
                    case "Cheese Wheel":
                        _item_display_list[i].sprite = _cheese_wheel;
                        break;
                    case "Pie":
                        _item_display_list[i].sprite = _pie;
                        break;
                    case "Stew":
                        _item_display_list[i].sprite = _stew;
                        break;
                }

            }
            else
            {
                _item_display_list[i].gameObject.SetActive(false);
            }

        }
    }
    public void Respawn()
    {
        //resets player transform to the most recent checkpoint
        _healthbar.value = GameController.Instance.Player._maxHealth;
        GameController.Instance.Player._health = GameController.Instance.Player._maxHealth;
        GameController.Instance.Player.transform.position = _respawnPoint.position;
        
    }
    public void SetRespawnPoint(Transform respawn)
    {
        //store the transform of the most recent checkpoint
        _respawnPoint = respawn;
    }
    
}
