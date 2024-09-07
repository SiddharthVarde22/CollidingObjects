using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : GenericSingleton<UpdateManager>
{
    private List<IUpdatable> m_ListOfUpdatables = new List<IUpdatable>();
    private List<ILateUpdatable> m_lateUpdatables = new List<ILateUpdatable>();
    private List<IJobUpdatable> m_jobUpdatables = new List<IJobUpdatable>();

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

    public static void SubscribeForLateUpdateCall(ILateUpdatable a_lateUpdatable)
    {
        Instance.m_lateUpdatables.Add(a_lateUpdatable);
    }

    public static void UnsubscribeFromLateUpdateCall(ILateUpdatable a_lateUpdatable)
    {
        Instance?.m_lateUpdatables.Remove(a_lateUpdatable);
    }

    public static void SubscribeForJobUpdateCall(IJobUpdatable a_jobUpdatable)
    {
        Instance.m_jobUpdatables.Add(a_jobUpdatable);
    }

    public static void UnsubscribeFromJobUpdateCall(IJobUpdatable a_jobUpdatable)
    {
        Instance?.m_jobUpdatables.Remove(a_jobUpdatable);
    }

    protected override void OnDestroy()
    {
        m_ListOfUpdatables.Clear();
        base.OnDestroy();
    }

    private void Update()
    {
        float l_deltaTime = Time.deltaTime;
        int l_updatablesCount = m_ListOfUpdatables.Count;
        IUpdatable l_updatable;
        for(int i = 0; i < l_updatablesCount; i++)
        {
            l_updatable = m_ListOfUpdatables[i];
            if(l_updatable != null && l_updatable.ShouldUpdate)
            {
                l_updatable.OnUpdateCalled(l_deltaTime);
            }
        }

        int l_jobUpdatablesCount = m_jobUpdatables.Count;
        IJobUpdatable l_jobUpdatable;
        for (int i = 0; i < l_jobUpdatablesCount; i++)
        {
            l_jobUpdatable = m_jobUpdatables[i];
            if (l_jobUpdatable != null && l_jobUpdatable.ShouldUpdate())
            {
                l_jobUpdatable.OnUpdateCalled(l_deltaTime);
            }
        }
    }

    private void LateUpdate()
    {
        float l_deltaTime = Time.deltaTime;

        int l_jobUpdatablesCount = m_jobUpdatables.Count;
        IJobUpdatable l_jobUpdatable;
        for (int i = 0; i < l_jobUpdatablesCount; i++)
        {
            l_jobUpdatable = m_jobUpdatables[i];
            if (l_jobUpdatable != null && l_jobUpdatable.ShouldUpdate())
            {
                l_jobUpdatable.OnJobCompleteCalled();
            }
        }

        int l_count = m_lateUpdatables.Count;
        ILateUpdatable l_lateUpdatable;

        for(int i = 0; i < l_count; i++)
        {
            l_lateUpdatable = m_lateUpdatables[i];
            if(l_lateUpdatable.ShouldUpdate())
            {
                l_lateUpdatable.OnLateUpdateCalled(l_deltaTime);
            }
        }
    }
}
