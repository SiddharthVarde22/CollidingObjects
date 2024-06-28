using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : GenericSingleton<UpdateManager>
{
    private List<IUpdatable> m_ListOfUpdatables = new List<IUpdatable>();

    public static void SubscribeForUpdateCall(IUpdatable a_updatable)
    {
        //if(Instance.m_ListOfUpdatables.Contains(a_updatable))
        //{
        //    Debug.LogError("Already exists in list");
        //    return;
        //}
        Instance.m_ListOfUpdatables.Add(a_updatable);
    }

    public static void UnsubscribeFromUpdateCall(IUpdatable a_updatable)
    {
        //if(Instance != null && Instance.m_ListOfUpdatables.Contains(a_updatable))
        //{
        //}
        //if(Instance == null)
        //{
        //    Debug.Log("Instance is null");
        //}
        Instance?.m_ListOfUpdatables.Remove(a_updatable);

    }

    protected override void OnDestroy()
    {
        m_ListOfUpdatables.Clear();
        base.OnDestroy();
    }

    private void Update()
    {
        for(int i = 0; i < m_ListOfUpdatables.Count; i++)
        {
            if(m_ListOfUpdatables[i] != null && m_ListOfUpdatables[i].ShouldUpdate)
            {
                m_ListOfUpdatables[i].OnUpdateCalled(Time.deltaTime);
            }
        }
    }
}
