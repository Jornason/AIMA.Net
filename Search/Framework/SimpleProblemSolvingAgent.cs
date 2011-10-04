namespace AIMA.Core.Search.Framework
{
    using System;
    using System.Collections.Generic;
    using AIMA.Core.Agent;
    using AIMA.Core.Agent.Impl;
    using AIMA.Core.Util;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.1, page 67.
     * <code>
     * function SIMPLE-PROBLEM-SOLVING-AGENT(percept) returns an action
     *   persistent: seq, an action sequence, initially empty
     *               state, some description of the current world state
     *               goal, a goal, initially null
     *               problem, a problem formulation
     *           
     *   state <- UPDATE-STATE(state, percept)
     *   if seq is empty then
     *     goal    <- FORMULATE-GOAL(state)
     *     problem <- FORMULATE-PROBLEM(state, goal)
     *     seq     <- SEARCH(problem)
     *     if seq = failure then return a null action
     *   action <- FIRST(seq)
     *   seq <- REST(seq)
     *   return action
     * </code>
     * Figure 3.1 A simple problem-solving agent. It first formulates a goal and a problem,
     * searches for a sequence of actions that would solve the problem, and then executes the actions
     * one at a time. When this is complete, it formulates another goal and starts over.<br>
     * 
     */

    /**
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class SimpleProblemSolvingAgent : AbstractAgent
    {

        // seq, an action sequence, initially empty
        private List<Action> seq = new List<Action>();

        //
        private bool formulateGoalsIndefinitely = true;

        private int maxGoalsToFormulate = 1;

        private int goalsFormulated = 0;

        public SimpleProblemSolvingAgent()
        {
            formulateGoalsIndefinitely = true;
        }

        public SimpleProblemSolvingAgent(int maxGoalsToFormulate)
        {
            formulateGoalsIndefinitely = false;
            this.maxGoalsToFormulate = maxGoalsToFormulate;
        }

        // function SIMPLE-PROBLEM-SOLVING-AGENT(percept) returns an action
        public override Action execute(Percept p)
        {
            Action action = NoOpAction.NO_OP;

            // state <- UPDATE-STATE(state, percept)
            updateState(p);
            // if seq is empty then do
            if (0 == seq.Count)
            {
                if (formulateGoalsIndefinitely
                        || goalsFormulated < maxGoalsToFormulate)
                {
                    if (goalsFormulated > 0)
                    {
                        notifyViewOfMetrics();
                    }
                    // goal <- FORMULATE-GOAL(state)
                    Object goal = formulateGoal();
                    goalsFormulated++;
                    // problem <- FORMULATE-PROBLEM(state, goal)
                    Problem problem = formulateProblem(goal);
                    // seq <- SEARCH(problem)
                    seq.AddRange(search(problem));
                    if (0 == seq.Count)
                    {
                        // Unable to identify a path
                        seq.Add(NoOpAction.NO_OP);
                    }
                }
                else
                {
                    // Agent no longer wishes to
                    // achieve any more goals
                    setAlive(false);
                    notifyViewOfMetrics();
                }
            }

            if (seq.Count > 0)
            {
                // action <- FIRST(seq)
                action = Util.first(seq);
                // seq <- REST(seq)
                seq = Util.rest(seq);
            }

            return action;
        }

        //
        // PROTECTED METHODS
        //
        protected abstract State updateState(Percept p);

        protected abstract Object formulateGoal();

        protected abstract Problem formulateProblem(Object goal);

        protected abstract List<Action> search(Problem problem);

        protected abstract void notifyViewOfMetrics();
    }
}