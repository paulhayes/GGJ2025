using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class CommandmentsView : MonoBehaviour
{
    [SerializeField] ActivityIconData m_iconData;

    [SerializeField] CommandmentView[] m_commandmentCards;

    CommandmentManager m_commandmentManager;
    Queue<int> _nextCardIndex = new();
    Dictionary<Activity, Texture> _activityIconMap = new();
    Dictionary<int, int> _commandMentIdToSelectedIndexMap = new();

    void Awake()
    {
        _activityIconMap.Add(Activity.Rest, m_iconData.RestingIcon);
        _activityIconMap.Add(Activity.BlowingBubbles, m_iconData.BlowingBubblesIcon);
        _activityIconMap.Add(Activity.Dancing, m_iconData.DancingIcon);
        _activityIconMap.Add(Activity.MakingBubbleTea, m_iconData.BubbleTeaIcon);
        _activityIconMap.Add(Activity.MixingSoap, m_iconData.WashingUpIcon);
        _activityIconMap.Add(Activity.Praying, m_iconData.PrayingIcon);
        _activityIconMap.Add(Activity.Pumping, m_iconData.PumpingIcon);
        _activityIconMap.Add(Activity.Recruiting, m_iconData.RecruitingIcon);

        _nextCardIndex.Enqueue(0);
        _nextCardIndex.Enqueue(1);
        _nextCardIndex.Enqueue(2);
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
        // GameObject commandmentView = Instantiate(m_commandmentViewPrefab);
        int nextIndex = _nextCardIndex.Dequeue();
        m_commandmentCards[nextIndex].gameObject.SetActive(true);
        _commandMentIdToSelectedIndexMap.Add(data.ID, nextIndex);

        for (int i = 0; i < 4; i += 1)
        {
            // data.activities[i];
            if (i < data.activities.Length)
            {
                m_commandmentCards[nextIndex].SetIconActive(i, true);
                m_commandmentCards[nextIndex].SetIcon(i, _activityIconMap[data.activities[i]]);
            }
            else
            {
                m_commandmentCards[nextIndex].SetIconActive(i, false);
            }
        }
    }

    public void OnCommandmentCompleted(CommandmentData data)
    {
        _nextCardIndex.Enqueue(_commandMentIdToSelectedIndexMap[data.ID]);
        _commandMentIdToSelectedIndexMap.Remove(data.ID);
    }
}
