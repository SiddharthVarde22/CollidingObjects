using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : GenericSingleton<UpdateManager>
{
    private List<IUpdatable> m_ListOfUpdatables = new List<IUpdatable>();
    private List<ILateUpdatable> m_LateUpdatables = new List<ILateUpdatable>();

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

    public static void SubscribeToLateUpdateCallback(ILateUpdatable a_lateUpdatable)
    {
        Instance.m_LateUpdatables.Add(a_lateUpdatable);
    }

    public static void UnSubscribeFromLateUpdate(ILateUpdatable a_lateUpdatable)
    {
        Instance?.m_LateUpdatables.Remove(a_lateUpdatable);
    }

    protected override void OnDestroy()
    {
        m_ListOfUpdatables.Clear();
        m_LateUpdatables.Clear();
        base.OnDestroy();
    }

    private void Update()
    {
        float l_deltaTime = Time.deltaTime;
        float l_updatablesCount = m_ListOfUpdatables.Count;
        IUpdatable l_updatable;
        for(int i = 0; i < l_updatablesCount; i++)
        {
            l_updatable = m_ListOfUpdatables[i];
            if(l_updatable != null && l_updatable.ShouldUpdate)
            {
                l_updatable.OnUpdateCalled(l_deltaTime);
            }
        }
    }

    private void LateUpdate()
    {
        float l_deltaTime = Time.deltaTime;
        int l_count = m_LateUpdatables.Count;
        ILateUpdatable l_lateUpdatable;

        for(int i = 0; i < l_count; i++)
        {
            l_lateUpdatable = m_LateUpdatables[i];
            if(l_lateUpdatable.ShouldUpdate())
            {
                l_lateUpdatable.OnLateUpdateCalled(l_deltaTime);
            }
        }
    }
}
