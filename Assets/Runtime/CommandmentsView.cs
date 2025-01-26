using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class CommandmentsView : MonoBehaviour
{
    [SerializeField] ActivityIconData m_iconData;

    [SerializeField] CommandmentView[] m_commandmentCards;

    CommandmentManager m_commandmentManager;

    Dictionary<int, CommandmentView> _commandMentIdToCommandmentView = new();

    void Awake()
    {


        m_commandmentManager = GameObject.FindFirstObjectByType<CommandmentManager>();
        if (!m_commandmentManager)
        {
            Debug.LogError("Unable to find command manager", this);
        }

        if (!m_iconData)
        {
            Debug.LogError("No icon data on commandments view");
        }

        m_commandmentManager.OnNewCommandmentAdded += OnNewCommandmentAdded;
        m_commandmentManager.OnCommandmentCompleted += OnCommandmentCompleted;

        foreach (CommandmentView view in m_commandmentCards)
        {
            view.gameObject.SetActive(false);
        }
    }

    public void OnNewCommandmentAdded(CommandmentData data)
    {
        var commandmentCard = Instantiate(m_commandmentCards[data.activities.Length],m_commandmentCards[data.activities.Length].transform.parent);
        commandmentCard.gameObject.SetActive(true);
        _commandMentIdToCommandmentView.Add(data.ID, commandmentCard);
        
        for (int i = 0; i < 4; i += 1)
        {
            commandmentCard.SetIconActive(i, i < data.activities.Length);            
        }
    }

    public void OnCommandmentCompleted(CommandmentData data)
    {
        Destroy(_commandMentIdToCommandmentView[data.ID].gameObject);
        _commandMentIdToCommandmentView.Remove(data.ID);
    }

    public void UpdateView()
    {
        var commandments = m_commandmentManager.Commandments;
        for(int i=0;i<commandments.Count;i++){
            var commandment = commandments[i];
            var view = _commandMentIdToCommandmentView[commandment.ID];
            
            for(int j=0;j<commandment.activities.Length;j++){
                view.SetIcon(j,commandment.activities[j],commandment.activityInProgress[j]);
                view.SetProgress(1-(commandment.time/commandment.duration));
            }
        }
    }

}
