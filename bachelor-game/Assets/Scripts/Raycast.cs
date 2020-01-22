using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    public float range = 100f;

    public Camera fpsCam;

    bool interacting = false;

    public GameObject interactionIcon;

    public bool inDialogue = false;

    bool cooldown = false;
    bool viewingInformation = false;


    public GameObject interactingWith;


    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.tag == "interactable")
            {
                interactionIcon.SetActive(!interacting);
                if (Input.GetButtonDown("Interact") && !inDialogue)
                {
                    interactingWith = hit.collider.gameObject.GetComponent<OpenUI>();
                    Debug.Log(hit.collider.gameObject.GetComponent<OpenUI>());

                    if (interactingWith.interactable)
                    {
                        interactingWith.ShowUIElement();
                    }
                    if (!interacting)
                    {
                        EnterInteraction();
                    }
                    else
                    {
                        ExitInteraction();
                    }
                }

            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }
        */


        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (interacting == false)
            {
                cooldown = true;

                if (hit.collider.gameObject.tag == "NPC" | hit.collider.gameObject.tag == "ViewableObject" | hit.collider.gameObject.tag == "InteractableObject")
                {
                    interactionIcon.SetActive(true);

                    if (Input.GetButtonDown("Interact"))
                    {
                        interactingWith = hit.collider.gameObject;
                        Debug.Log(interactingWith);

                        if (interactingWith.tag == "NPC")
                        {
                            EnterInteraction();
                            Cursor.lockState = CursorLockMode.None;
                            interactingWith.GetComponent<EnterDialogue>().StartDialogue();
                        }
                        else if (interactingWith.tag == "ViewableObject")
                        {
                            EnterInteraction();
                            interactingWith.GetComponent<ViewObject>().EnterView();
                        }
                        else if (interactingWith.tag == "InteractableObject")
                        {
                            if (interactingWith.GetComponent<InteractableObject>().Interaction())
                            {
                                Cursor.lockState = CursorLockMode.None;
                                EnterInteraction();
                            }

                        }
                        else
                        {
                            interactingWith = null;
                        }
                    }
                }
            }
        }
        else { interactionIcon.SetActive(false); }

        if (interacting == true && cooldown == false)
        {
            if (interactingWith.tag =="ViewableObject")
            {
                if (Input.GetMouseButtonDown(0) && !cooldown && interacting)
                {
                    cooldown = true;
                    interactingWith.GetComponent<ViewObject>().ViewInformation(viewingInformation);
                    viewingInformation = !viewingInformation;
                    
                }

            }


            cooldown = true;

            if (Input.GetButtonDown("Escape"))
            {


                if (interactingWith.tag == "NPC")
                {
                    interactingWith.GetComponent<EnterDialogue>().EndDialogue();
                }
                else if (interactingWith.tag == "ViewableObject")
                {
                    interactingWith.GetComponent<ViewObject>().QuitView();
                }
                interactingWith = null;
                interacting = false;
                transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            }

            if (Input.GetButtonDown("Interact"))
            {
                if (interactingWith.tag == "ViewableObject")
                {
                    transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                    if (interactingWith.GetComponent<ViewObject>().item != null)
                    {
                        if (interactingWith.GetComponent<ViewObject>().item.collectible == true)
                        {
                            //collect item
                            Debug.Log(interactingWith.GetComponent<ViewObject>().item.name + " collected.");
                            interactingWith.GetComponent<ViewObject>().CollectItem();
                        }
                    }
                    ExitInteraction();
                }
            }
        }

        cooldown = false;

    }

    public void ExitInteraction()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //interactingWith.GetComponent<ViewObject>().QuitView();
        transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        interactingWith = null;
        interacting = false;
        
    }
    void EnterInteraction()
    {
        
        Cursor.visible = true;
        interactionIcon.SetActive(false);
        transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        interacting = true;
        
    }

    void OpenMainMenu(bool open)
    {

    }
}
