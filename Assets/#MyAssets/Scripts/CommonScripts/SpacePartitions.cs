using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePartitions : GenericSingleton<SpacePartitions>
{
    [SerializeField]
    float m_lengthOfSpace = 1;
    [SerializeField]
    float m_extraDistonceToAdd = 0.1f;

    int m_xLength, m_yLength;
    Vector2 m_topLeftCorner;
    SpaceData[,] m_spaces;
    private void Start()
    {
        CalculateSize();
        m_spaces = new SpaceData[m_yLength, m_xLength];
        PopulateSpaceData();
    }

    void CalculateSize()
    {
        Vector2 l_bottomLeftBoundry, l_topRightBoundry;
        (l_bottomLeftBoundry, l_topRightBoundry) = BoundriesCalculator.GetBounries;
        m_topLeftCorner.x = l_bottomLeftBoundry.x;
        m_topLeftCorner.y = l_topRightBoundry.y;

        float l_xLength = (l_topRightBoundry.x - l_bottomLeftBoundry.x) / m_lengthOfSpace;
        float l_yLength = (l_topRightBoundry.y - l_bottomLeftBoundry.y) / m_lengthOfSpace;

        m_xLength = (int)l_xLength + 1;
        m_yLength = (int)l_yLength + 1;

        //if (m_xLength < l_xLength)
        //{
        //    m_xLength++;
        //}

        //if (m_yLength < l_yLength)
        //{
        //    m_yLength++;
        //}
    }

    void PopulateSpaceData()
    {
        Vector2 l_startCorner = m_topLeftCorner;
        for(int i = 0; i < m_yLength; i++)
        {
            for(int j = 0; j < m_xLength; j++)
            {
                m_spaces[i, j] = new SpaceData();
                m_spaces[i, j].Init(l_startCorner, m_lengthOfSpace);
                l_startCorner.x += m_lengthOfSpace;
            }
            l_startCorner.x = m_topLeftCorner.x;
            l_startCorner.y -= m_lengthOfSpace;
        }
    }

    public static SpaceData GetSpace(Vector3 a_position)
    {
        SpaceData l_spaceToReturn = null;
        int l_row = 0, l_column = 0;
        
        for(int i = 0; i < Instance.m_yLength; i++)
        {
            if(Instance.m_spaces[i, l_column].CheckIf_Y_InSpace(a_position.y))
            {
                l_row = i;
                break;
            }
        }

        for(int i = 0; i < Instance.m_xLength; i++)
        {
            if(Instance.m_spaces[l_row, i].CheckIf_X_IsInSpace(a_position.x))
            {
                l_column = i;
                break;
            }
        }

        l_spaceToReturn = Instance.m_spaces[l_row, l_column];

        //Debug.Log("Position doesnot exist in Any space");
        return l_spaceToReturn;
    }
}
