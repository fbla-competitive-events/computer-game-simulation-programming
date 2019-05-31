using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Where all of the methods that the dialogues use house
/// 
/// If you want to do a response method, do this signiature:
/// public static List<Response> MethodName(List<Repsponse> responses, Dialogue caller)
/// 
/// If you want to do a dialogue method, do this signiature:
/// public static Dialogue MethodName(List<Dialogue> responses)
/// </summary>
public static class DialogueMethod
{ 
    /// <summary>
    /// All of the methods that Fuller uses in his dialogue
    /// </summary>
    public static class Fuller
    {
        /// <summary>
        /// When asked about fundraising
        /// </summary>
        /// <param name="possibleDialogues">The possible dialogues that can be returned</param>
        /// <returns></returns>
        public static Dialogue Fundraising(List<Dialogue> possibleDialogues)
        {                        
            int progress;

            //if there is already a cards task, display the second possible dialogue with how many cards the player has
            if (TaskManager.Instance.GetProgressLeft(TCards.taskName, TCards.ObjectiveName, out progress))
            {
                Dialogue dialogue = possibleDialogues[1];
                dialogue.DialogueText[0] = string.Format(dialogue.DialogueText[0], progress);
                return dialogue;
            }

            //If there is no task, create one
            TaskManager.Instance.NewTask(new TCards());
            return possibleDialogues[0];
        }

        /// <summary>
        /// When the player asks about recruiting
        /// </summary>
        /// <param name="possibleDialogues"></param>
        /// <returns></returns>
        public static Dialogue Recruit(List<Dialogue> possibleDialogues)
        {
            return staticDialogue(new TRecruit(), possibleDialogues);
        }

        /// <summary>
        /// When the player asks about how to progress on event
        /// </summary>
        /// <param name="possibleDialogues"></param>
        /// <returns></returns>
        public static Dialogue Progress(List<Dialogue> possibleDialogues)
        {
            return staticDialogue(new TProgress(), possibleDialogues);
        }

        /// <summary>
        /// When the player asks when the next event is
        /// </summary>
        /// <param name="possibleDialogues"></param>
        /// <returns></returns>
        public static Dialogue Event(List<Dialogue> possibleDialogues)
        {            
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();            
            if (gameManager.ReadyForEvent)
            {
                return possibleDialogues[1];
            }            
            return possibleDialogues[0];
        }

        /// <summary>
        /// When the player wants to go to the event
        /// </summary>
        /// <param name="possibleDialogues"></param>
        /// <returns></returns>
        public static Dialogue GotoEvent(List<Dialogue> possibleDialogues)
        {
            TaskManager.Instance.AddProgress(TReady.TaskName, TReady.ObjectiveName, TReady.IncrementAmount);
            DialogueTrigger.AddOnCloseEvent(new DialogueManager.Event(() => 
            {
                GameObject.FindObjectOfType<GameManager>().GoToEvent();
            }));
            return null;
        }

        //This is called whenever a Method just creates a task and only has one possible dialogue
        private static Dialogue staticDialogue(Task task, List<Dialogue> possibleDialogues)
        {
            TaskManager taskManager = GameObject.FindGameObjectWithTag("TaskBox").GetComponent<TaskManager>();
            //Create a new task. If a task already exists, the method NewTask knows what to do
            taskManager.NewTask(task);
            return possibleDialogues[0];
        }
    }

    public static class Tasks
    {
        public static Dialogue Membership(List<Dialogue> possibleDialogues)
        {
            DialogueManager.Instance.CurrentDialogue.Caller.AnsweredPlayerMembership = true;
            if (UnityEngine.Random.Range(0, 2) == 1 || Player.Instance.Debug)
            {                
                //Becomes a member, so update the task
                UnityEngine.Object.FindObjectOfType<TaskManager>().AddProgress(TRecruit.taskName, TRecruit.ObjectiveName, TRecruit.IncrementAmount);
                
                return possibleDialogues[0];
            }
            return possibleDialogues[1];
        }
        public static Dialogue Cards(List<Dialogue> possibleDialogues)
        {
            DialogueManager.Instance.CurrentDialogue.Caller.AnsweredPlayerCards = true;
            if (UnityEngine.Random.Range(0, 2) == 1 || Player.Instance.Debug)
            {                
                //Buys a card, so update the task.
                UnityEngine.Object.FindObjectOfType<TaskManager>().AddProgress(TCards.taskName, TCards.ObjectiveName, TCards.IncrementAmount);
                
                return possibleDialogues[0];
            }
            return possibleDialogues[1];
        }
        public static Dialogue Service(List<Dialogue> possibleDialoges)
        {            
            GameObject.FindObjectOfType<TaskManager>().NewTask(new TTrash());
            return possibleDialoges[0];
        }
    }
    public static class Classmate
    {
       
        public static List<Response> ClassmateResponses(List<Response> possibleResponses, Dialogue caller)
        {
            List<Response> l = new List<Response>();
            l.Add(possibleResponses[0]);
            l.Add(possibleResponses[1]);
            if (!caller.Caller.AnsweredPlayerCards && GameObject.FindObjectOfType<TaskManager>().ContainsTask(typeof(TCards)))
            {
                l.Add(possibleResponses[2]);                          
            }                
            if (!caller.Caller.AnsweredPlayerMembership && GameObject.FindObjectOfType<TaskManager>().ContainsTask(typeof(TRecruit)))
            {
                l.Add(possibleResponses[3]);                             
            }                
            return l;
        }
    }

    public static class Teacher
    {
        public static List<Response> TeacherResponses(List<Response> possibleResponses, Dialogue caller)
        {
            List<Response> l = new List<Response>();
            l.Add(possibleResponses[0]);
            l.Add(possibleResponses[2]);
            if (!caller.Caller.AnsweredPlayerCards && GameObject.FindObjectOfType<TaskManager>().ContainsTask(typeof(TCards)))
            {
                l.Add(possibleResponses[1]);                
            }                           
            return l;
        }
    }

    public static class Computer
    {
        public static Dialogue Progress(List<Dialogue> possibleDialogues)
        {

            DialogueTrigger.AddOnCloseEvent(() =>
            {
                string stage;
                int educationLevel = Player.Instance.ProgressLevel;
                if (educationLevel < 5)
                {
                    stage = "Region";
                }
                else if (educationLevel < 7)
                {
                    stage = "State";
                }
                else
                {
                    stage = "Nationals";
                }
                QuestionTrigger.StartQuestionGame(stage + "ProgressData.json", (int score) =>
                {
                    Player.Instance.GetComponent<Player>().Progress += score;
                    Player.Instance.CanWalk = true;
                });
            });
            return null;
        }
        public static Dialogue Education(List<Dialogue> possibleDialogues)
        {
            DialogueTrigger.AddOnCloseEvent(() => 
            {
                string stage;
                int educationLevel = Player.Instance.EducationLevel;
                if (educationLevel < 5)
                {
                    stage = "Region";
                }
                else if (educationLevel < 7)
                {
                    stage = "State";
                }
                else
                {
                    stage = "Nationals";
                }
                QuestionTrigger.StartQuestionGame(stage + "EducationData.json", (int score) => 
                {
                    Player.Instance.GetComponent<Player>().Education += score;
                    Player.Instance.CanWalk = true;
                });
            });
            return null;
        }        
    }

    public static class FBLAOfficer
    {
        public static List<Response> OfficerResponses(List<Response> responses, Dialogue caller)
        {
            List<Response> l = new List<Response>();

            //If the officer is asking is there are multiple players, return the "Yes" or "No" responses
            if (DialogueManager.Instance.ContinueToResponses == 2)
            {
                l.Add(responses[0]);
                l.Add(responses[1]);
                return l;
            }

            //If not, return the rest of the responses
            for (int i = 2; i < responses.Count; i++)
            {
                l.Add(responses[i]);
            }

            return l;
        }

        public static Dialogue YesReponse(List<Dialogue> dialogues)
        {
            GameManager.Instance.IsMultiplayer = true;            
            return null;
        }

        public static Dialogue NoReponse(List<Dialogue> dialogues)
        {
            GameManager.Instance.IsMultiplayer = false;            
            return null;
        }

    }
    

    /// <summary>
    /// Goes through each NestedClass and tries to find the method with the given name. Calls the method and returns its dialogue
    /// </summary>
    /// <param name="MethodName">The name of the method to invoke</param>
    /// <param name="parameters">The list of possible dialogues that the method takes as a paramter.</param>
    /// <returns>Returns the dialogue returned by the invoked method.</returns>
    public static object InvokeMethod(string MethodName, object[] parameters)
    {
        MethodInfo method = null;
        foreach (System.Type type in typeof(DialogueMethod).GetNestedTypes())
        {
            method = type.GetMethod(MethodName);
            if (method != null)
            {
                break;
            }
        }
        return method.Invoke(null, parameters);
    }

    /// <summary>
    /// Gets all the methods names that are used for when a response is selected in the message box.
    /// </summary>   
    /// <returns>A list of all the method names.</returns>
    public static List<string> EnterMethods()
    {
        return getAllMethods(typeof(List<Dialogue>));    
    }

    /// <summary>
    /// Gets all of the methods names for the dialogues
    /// </summary>
    /// <returns></returns>
    public static List<string> DialogueMethods()
    {
        return getAllMethods(typeof(List<Response>), typeof(Dialogue));
    }

    /// <summary>
    /// Gets all of the methods in this class
    /// </summary>
    /// <param name="parameterTypes">The parameters to look for when returning a method</param>
    /// <returns></returns>
    private static List<string> getAllMethods(params Type[] parameterTypes)
    {
        List<MethodInfo> methods = new List<MethodInfo>();
        List<string> methodStrings = new List<string>();
        foreach (Type type in typeof(DialogueMethod).GetNestedTypes())
        {
            List<MethodInfo> l = getMethods(type, parameterTypes);
            foreach (MethodInfo m in l)
            {
                methodStrings.Add(string.Format("{0}/{1}", type.Name, m.Name));
            }           
        }        
        return methodStrings;
    }

    /// <summary>
    /// Gets all the methods in the class classType that has parameters of parameterTypes
    /// </summary>
    /// <param name="classType">The type to get all the methods from</param>
    /// <param name="parameterTypes">The parameters of the method</param>
    /// <returns>A list of all the methods.</returns>
    private static List<MethodInfo> getMethods(Type classType, Type[] parameterTypes)
    {
        List<MethodInfo> methods = new List<MethodInfo>();        
        MethodInfo[] allMethods = classType.GetMethods();
        foreach (MethodInfo method in allMethods)
        {
            ParameterInfo[] parameters = method.GetParameters();
            List<Type> lParameterTypes = new List<Type>(parameterTypes);
            //See if the parameters of the method match the given parameterTypes
            bool flag = (parameters.Length == lParameterTypes.Count);
            for (int i = 0; i < parameters.Length && flag; i++)
            {
                Predicate<Type> predi = x => x == parameters[i].ParameterType;
                if (!lParameterTypes.Exists(predi))
                {
                    flag = false;
                    break;
                }
                lParameterTypes.Remove(lParameterTypes.Find(predi));
            }            
            if (flag)
            {
                methods.Add(method);
            }
        }
        return methods;
    }
}
