using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject enablerGameObject;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        enablerGameObject.SetActive(true);
    }

    private void Hide()
    {
        enablerGameObject.SetActive(false);
    }
}
