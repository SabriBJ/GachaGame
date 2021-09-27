using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    #region Attributs
    public delegate bool Test();

    Test m_Tests;
    Type m_Destination;
    #endregion

    #region Getters/Setters
    public Type Destination
    {
        get
        {
            return m_Destination;
        }
    }
    #endregion

    #region Constructors
    public Transition(Test _TestFunctions, Type _Destination)
    {
        m_Tests = _TestFunctions;
        m_Destination = _Destination;
    }

    public Transition(Test[] _TestFunctions, Type _Destination)
    {
        for (int i = 0; i < _TestFunctions.Length; i++)
        {
            m_Tests += _TestFunctions[i];
        }
        m_Destination = _Destination;
    }

    #endregion

    #region Public Methods
    public void AddTestFonction(Test _TestFunction)
    {
        m_Tests += _TestFunction;
    }

    public void RemoveTestFonction(Test _TestFunction)
    {
        m_Tests -= _TestFunction;
    }

    public bool IsTransitionTrue()
    {
        foreach (Test t in m_Tests.GetInvocationList())
        {
            if (t())
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
