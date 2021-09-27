using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    #region Attributs
    bool m_HasEnteredCurrentState = false;
    State m_CurrentState;
    State m_PreviousState;
    Dictionary<Type, State> m_States;
    #endregion

    #region Constructor
    /// <summary>
    /// The starting state is the first state of the List
    /// If <param "_States"></param> contains two instance of the same Type only the first one will be added 
    /// </summary>
    /// <param name="_States"> List of State to add to the machine </param>
    public FiniteStateMachine(List<State> _States)
    {
        m_States = new Dictionary<Type, State>();
        m_CurrentState = null;
        m_PreviousState = null;

        if (_States != null && _States.Count > 0)
        {
            for (int i = 0; i < _States.Count; i++)
            {
                if (!m_States.ContainsKey(_States[i].GetType()))
                {
                    m_States.Add(_States[i].GetType(), _States[i]);
                }
            }
            m_CurrentState = _States[0];
        }
        else //if there is no state add one default State
        {
            State s = new State(null);
            m_States.Add(s.GetType(), s);
            m_CurrentState = s;
        }
    }
    #endregion

    #region Getters/Setters

    public Dictionary<Type, State> States
    {
        get
        {
            return m_States;
        }
    }

    public State CurrentState
    {
        get
        {
            return m_CurrentState;
        }
    }

    public State PreviousState
    {
        get
        {
            return m_PreviousState;
        }
    }

    #endregion

    #region Public Methods
    public void AddState(State _State)
    {
        if (m_States.ContainsKey(typeof(State))) // remove placeholder default state if there's one
        {
            m_States.Remove(typeof(State));
            m_PreviousState = m_CurrentState;
            m_CurrentState = _State;
        }
        m_States.Add(_State.GetType(), _State);
    }

    #endregion

    #region Private Methods

    private bool CanSwitchToState(Type _State)
    {
        return m_States.ContainsKey(_State) && _State != m_CurrentState.GetType();
    }

    private void SwitchToState(Type _State)
    {
        if (CanSwitchToState(_State))
        {
            m_PreviousState = m_CurrentState;
            m_States.TryGetValue(_State, out m_CurrentState);
        }
    }

    public void UpdateStep()
    {
        if (!m_HasEnteredCurrentState)
        {
            m_CurrentState.Enter();
            m_HasEnteredCurrentState = true;
        }
        else
        {
            Type t = m_CurrentState.Update();

            if (CanSwitchToState(t))
            {
                m_HasEnteredCurrentState = false;
                m_CurrentState.Exit();
                SwitchToState(t);
            }
        }
    }

    #endregion
}
