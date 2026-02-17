using System;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelUI;

    [Header("Player")]
    public PlayerMovement playerMovement; // arrastra aquí tu script de movimiento del Player
    public PlayerLook playerLook;

    private bool playerInside = false;
    private bool noActivar = false;

    private void Start()
    {
        if (panelUI != null) panelUI.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (noActivar) return;
        if (!other.gameObject.CompareTag("Player")) return;

        playerInside = true;
        OpenPanel();
    }



    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        playerInside = false;
        // Opcional: si quieres que al alejarse se cierre solo
        // ClosePanel();
    }

    public void OpenPanel()
    {
        if (!playerInside) return;

        if (panelUI != null) panelUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.SetCanMove(false);
        if (playerLook  != null)
            playerLook.SetCanMove(false);


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        Debug.Log("ClosePanel");
        if (panelUI != null) panelUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.SetCanMove(true);
        if (playerLook  != null)
            playerLook.SetCanMove(true);
        Cursor.lockState = CursorLockMode.Locked; // o Confined
        Cursor.visible = false;
        StartCoroutine(NoActivarPanel());
    }


    IEnumerator NoActivarPanel()
    {
        noActivar = true;
        playerInside = false;
        yield return new WaitForSeconds(5f);
        ;
        noActivar = false;

        Debug.Log("Han pasado 2 segundos");
    }
}
