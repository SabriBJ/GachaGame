using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    #region Attributs

    bool m_HasBeenEntered = false;
    bool m_HasBeenExited = false;
    List<Transition> m_Transitions;


    #endregion

    #region Constructors
    public State(List<Transition> _Transitions)
    {
        if (m_Transitions == null)
        {
            m_Transitions = new List<Transition>();
        }
        else
        {
            m_Transitions = _Transitions;
        }
    }

    public State()
    {
        m_Transitions = new List<Transition>();
    }

    #endregion

    #region Virtual Methods
    protected virtual void OnFirstEnter() { }

    protected virtual void OnEnter() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnFirstExit() { }

    protected virtual void OnExit() { }
    #endregion

    #region Public Methods
    public void Enter()
    {
        if (!m_HasBeenEntered)
        {
            OnFirstEnter();
            m_HasBeenEntered = true;
        }

        OnEnter();
    }

    public System.Type Update()
    {
        OnUpdate();
        if (m_Transitions.Count == 0)
        {
            return GetType();
        }

        for (int i = 0; i < m_Transitions.Count; i++)
        {
            if (m_Transitions[i].IsTransitionTrue())
            {
                return m_Transitions[i].Destination;
            }
        }

        return GetType();
    }

    public void Exit()
    {
        if (!m_HasBeenExited)
        {
            OnFirstExit();
            m_HasBeenExited = true;
        }

        OnExit();
    }

    public void AddTransition(Transition _Transition)
    {
        if (!m_Transitions.Contains(_Transition))
        {
            m_Transitions.Add(_Transition);
        }
    }

    public void RemoveTransition(Transition _Transition)
    {
        if (m_Transitions.Contains(_Transition))
        {
            m_Transitions.Remove(_Transition);
        }
    }

    #endregion

}
