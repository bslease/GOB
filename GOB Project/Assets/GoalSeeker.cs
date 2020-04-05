﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeeker : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;

    // Start is called before the first frame update
    void Start()
    {
        // my inital motives/goals
        mGoals = new Goal[2];
        mGoals[0] = new Goal("Eat", 4);
        mGoals[1] = new Goal("Sleep", 3);

        // the actions I know how to do
        mActions = new Action[4];
        mActions[0] = new Action("eat some raw food", "Eat", -3f);
        mActions[1] = new Action("eat a snack", "Eat", -2f);
        mActions[2] = new Action("sleep in the bed", "Sleep", -4f);
        mActions[3] = new Action("sleep on the sofa", "Sleep", -2f);

        Debug.Log("Ready. Hit E to do something.");
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach(Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        Debug.Log(goalString);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("-- INITIAL GOALS --");
            PrintGoals();

            Action bestThingToDo = ChooseAction(mActions, mGoals);
            Debug.Log("-- BEST ACTION --");
            Debug.Log("I think I will " + bestThingToDo.name);

            // do the thing
            foreach(Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
            }

            Debug.Log("-- NEW GOALS --");
            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        // find the most valuable goal to try and fulfill
        Goal topGoal = goals[0];
        foreach (Goal goal in goals)
        {
            if (goal.value > topGoal.value)
            {
                topGoal = goal;
            }
        }
        Debug.Log("My most pressing need is to " + topGoal.name);

        // find the best action to take
        Action bestAction = actions[0];
        float bestUtility = -actions[0].GetGoalChange(topGoal);

        foreach (Action action in actions)
        {
            // we invert the change because a low change value is good (we
            // want to reduce the value for the goal) but utilities are
            // typically scaled so high values are good.
            float utility = -action.GetGoalChange(topGoal);

            // we look for the lowest change (highest utility)
            if (utility > bestUtility)
            {
                bestUtility = utility;
                bestAction = action;
            }
        }

        // return the best action to be carried out
        return bestAction;
    }
}
