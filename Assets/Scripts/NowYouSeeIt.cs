using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NowYouSeeIt : MonoBehaviour
{
    public enum character
    {
        hoshi,
        hawking,
        ivy,
        greene
    }

    public EventSystem eventSystem;
    public character questCharacter;
    public GameObject beforeQuest;
    public GameObject afterQuest;

    // Start is called before the first frame update
    void Start()
    {
        getEventSystem();
        if (eventSystem != null)
        {
            Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
            if (questCharacter == character.hoshi)
            {
                if (flowchart.GetStringVariable("hoshi_state") == "QUEST_COMPLETE")
                {
                    afterQuest.SetActive(true);
                    beforeQuest.SetActive(false);
                }
                else
                {
                    beforeQuest.SetActive(true);
                    afterQuest.SetActive(false);
                }
            }
            if (questCharacter == character.hawking)
            {
                if (flowchart.GetStringVariable("hawking_state") == "QUEST_COMPLETE")
                {
                    afterQuest.SetActive(true);
                    beforeQuest.SetActive(false);
                }
                else
                {
                    beforeQuest.SetActive(true);
                    afterQuest.SetActive(false);
                }
            }
            if (questCharacter == character.ivy)
            {
                if (flowchart.GetStringVariable("ivy_state") == "QUEST_COMPLETE")
                {
                    afterQuest.SetActive(true);
                    beforeQuest.SetActive(false);
                }
                else
                {
                    beforeQuest.SetActive(true);
                    afterQuest.SetActive(false);
                }
            }
            if (questCharacter == character.greene)
            {
                if (flowchart.GetStringVariable("greene_state") == "QUEST_COMPLETE")
                {
                    afterQuest.SetActive(true);
                    beforeQuest.SetActive(false);
                }
                else
                {
                    beforeQuest.SetActive(true);
                    afterQuest.SetActive(false);
                }
            }
        }
    }

    void getEventSystem()
    {
        if (GameObject.FindGameObjectWithTag("EventSystem") != null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }
        else
        {
            Debug.Log("event system is not hooked up.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
