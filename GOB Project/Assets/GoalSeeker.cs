using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeeker : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // my inital motives/goals
        mGoals = new Goal[3];
        mGoals[0] = new Goal("Eat", 4);
        mGoals[1] = new Goal("Sleep", 3);
        mGoals[2] = new Goal("Bathroom", 3);

        //// the actions I know how to do
        //mActions = new Action[4];
        //mActions[0] = new Action("eat some raw food"), "Eat", -3f);
        //mActions[1] = new Action("eat a snack", "Eat", -2f);
        //mActions[2] = new Action("sleep in the bed", "Sleep", -4f);
        //mActions[3] = new Action("sleep on the sofa", "Sleep", -2f);

        // the actions I know how to do
        mActions = new Action[6];
        mActions[0] = new Action("eat some raw food");
        mActions[0].targetGoals.Add(new Goal("Eat", -3f));
        mActions[0].targetGoals.Add(new Goal("Sleep", +2f));
        mActions[0].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[1] = new Action("eat a snack");
        mActions[1].targetGoals.Add(new Goal("Eat", -2f));
        mActions[1].targetGoals.Add(new Goal("Sleep", -1f));
        mActions[1].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[2] = new Action("sleep in the bed");
        mActions[2].targetGoals.Add(new Goal("Eat", +2f));
        mActions[2].targetGoals.Add(new Goal("Sleep", -4f));
        mActions[2].targetGoals.Add(new Goal("Bathroom", +2f));

        mActions[3] = new Action("sleep on the sofa");
        mActions[3].targetGoals.Add(new Goal("Eat", +1f));
        mActions[3].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[3].targetGoals.Add(new Goal("Bathroom", +1f));

        mActions[4] = new Action("drink a soda");
        mActions[4].targetGoals.Add(new Goal("Eat", -1f));
        mActions[4].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[4].targetGoals.Add(new Goal("Bathroom", +3f));

        mActions[5] = new Action("visit the bathroom");
        mActions[5].targetGoals.Add(new Goal("Eat", 0f));
        mActions[5].targetGoals.Add(new Goal("Sleep", 0f));
        mActions[5].targetGoals.Add(new Goal("Bathroom", -4f));

        // the rate my goals change just as a result of time passing
        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Eat", +4f));
        mChangeOverTime.targetGoals.Add(new Goal("Sleep", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Bathroom", +2f));

        Debug.Log("Starting clock. One hour will pass every " + TICK_LENGTH + " seconds.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Hit E to do something.");
    }

    void Tick()
    {
        // apply change over time
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            //Debug.Log(mChangeOverTime.GetGoalChange(goal));
            goal.value = Mathf.Max(goal.value, 0);
        }

        // print results
        PrintGoals();
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach(Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        goalString += "Discontentment: " + CurrentDiscontentment();
        Debug.Log(goalString);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("-- INITIAL GOALS --");
            //PrintGoals();

            Action bestThingToDo = ChooseAction(mActions, mGoals);
            //Debug.Log("-- BEST ACTION --");
            Debug.Log("I think I will " + bestThingToDo.name);

            // do the thing
            foreach(Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
            }

            //Debug.Log("-- NEW GOALS --");
            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        // ----- simple selection ---------
        // -- find the most pressing goal
        // -- choose the action with the greatest impact on that goal
        // --------------------------------

        //// find the most valuable goal to try and fulfill
        //Goal topGoal = goals[0];
        //foreach (Goal goal in goals)
        //{
        //    if (goal.value > topGoal.value)
        //    {
        //        topGoal = goal;
        //    }
        //}
        //Debug.Log("My most pressing need is to " + topGoal.name);

        //// find the best action to take
        //Action bestAction = actions[0];
        //float bestUtility = -actions[0].GetGoalChange(topGoal);

        //foreach (Action action in actions)
        //{
        //    // we invert the change because a low change value is good (we
        //    // want to reduce the value for the goal) but utilities are
        //    // typically scaled so high values are good.
        //    float utility = -action.GetGoalChange(topGoal);

        //    // we look for the lowest change (highest utility)
        //    if (utility > bestUtility)
        //    {
        //        bestUtility = utility;
        //        bestAction = action;
        //    }
        //}
        //// return the best action to be carried out
        //return bestAction;

        // ----- utitiliy ---------
        // -- find the action leading to the 
        // -- lowest discontentment
        // --------------------------------

        // find the action leading to the lowest discontentment
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);
            //Debug.Log("Maybe I should " + action.name + ". Resulting discontentment = " + thisValue);
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        // keep a running total
        float discontentment = 0f;

        // loop through each goal
        foreach(Goal goal in goals)
        {
            // calculate the new value after the action
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            // get the discontentment of this value
            discontentment += goal.GetDiscontentment(newValue);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach(Goal goal in mGoals)
        {
            total += (goal.value * goal.value);
        }
        return total;
    }
}
