using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEnemy
{

}

public class Enemy: Unit
{
    List<State> states = new List<State>();
    public List<Vector2> bounds;
    public int enemyLevel;
    public Manager mng;
    public event AlarmCons FoundPlayer;
    public delegate void AlarmCons();




    private void OnEnable()
    {

    }


    private void Awake()
    {
        mng = GameObject.Find("Manager").GetComponent<Manager>();
        State patrolling = new State("patrolling", 1);
        State chasing = new State("chasing", 2);
        states.Add(patrolling);
        states.Add(chasing);
        try
        {
            FindState("patrolling");
        }
        catch (Exception e)
        {
            Debug.Log("State with this name doesn't exist");
        }
        CurrentState = (int)States.patrolling;

    }
    State FindState(string name)
    {
        State stateToReturn = null;
        foreach(State state in states)
        {
            if(state.name == name)
            {
                stateToReturn = state;
            }
        }
        if (stateToReturn == null)
        {
            throw new Exception("State doesn't exist");
        }
        return null;
    }
    void Start()
    {
        State patrolling = new State("patrolling", 1);
        State chasing = new State("chasing", 2);
        states.Add(patrolling);
        states.Add(chasing);
        Debug.Log("wow");
        try
        {
            FindState("patrollin");
        }
        catch (Exception e)
        {
            Debug.Log("State with this name doesn't exist");
        }
        
    }

    void Update()
    {
        
    }
    public void StartChasing()
    {
        CurrentState = (int)States.chasing;
        FoundPlayer();
    }
    public class State
    {
        public State(string name, float speed)
        {
            this.name = name;
            this.speed = speed;
        }
        public string name;
        public float speed;
    } 
}
