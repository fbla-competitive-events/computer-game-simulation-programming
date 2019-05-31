#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[CustomPropertyDrawer(typeof(Dialogue))]
public class DialogueDrawer : PropertyDrawer
{

    ///<summary>
    ///The height of a single line
    ///</summary>    
    const float k_SingleLineHeight = 16f;
    /// <summary>
    /// The width of a label
    /// </summary>
    const float k_LabelWidth = 60f;
    /// <summary>
    /// The default element height
    /// </summary>
    const float k_DefaultElementHeight = 92f;
    /// <summary>
    /// The padding between two dialogues
    /// </summary>
    const float k_PaddingBetweenRules = 13f;
    /// <summary>
    /// The height of a line plus the padding
    /// </summary>
    const float k_NewLine = k_SingleLineHeight + 5f;    

    /// <summary>
    /// The dialogue that is getting serialized
    /// </summary>
    Dialogue target;

    /// <summary>
    /// The parent of the target
    /// </summary>
    object parent;

    /// <summary>
    /// A reference to the dialogue editor
    /// </summary>
    DialogueEditor rootParent;

    /// <summary>
    /// A serialized version of target
    /// </summary>
    SerializedProperty property;

    /// <summary>
    /// The height of this drawer
    /// </summary>
    float height;

    /// <summary>
    /// The method used if the type of the dialogue is method
    /// </summary>
    UnityEvent typeMethod;
    
    /// <summary>
    /// Draws the serialized dialogue
    /// </summary>
    /// <param name="position">The position of the dialogue</param>
    /// <param name="property">A serialized version of the dialogue</param>
    /// <param name="label">The label</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {                
        //This is the y value of all of the things drawn
        float y = position.yMin + 2;
                
        //Initializes the reorderable list
        Initialize(property, position);

        //Tells if the dialogue has a parent or not
        bool hasParent = false;

        //If the parent is a response, then make the target's name its name
        if (parent is Response)
        {
            target.Name = ((Response)parent).Name;
            hasParent = true;
        }   
        
        //Label for the name    
        EditorGUI.BeginDisabledGroup(hasParent);
        GUI.Label(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Name");
        target.Name = EditorGUI.TextField(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), target.Name);
        EditorGUI.EndDisabledGroup();
        y += k_NewLine;

        ////Field for the dialogue text size
        //EditorGUI.BeginChangeCheck();
        //int size = target.DialogueText.Length;
        //GUI.Label(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Text Size");
        //size = EditorGUI.DelayedIntField(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), size);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    Array.Resize<string>(ref target.DialogueText, Math.Max(1, size));
        //}
        //y += k_NewLine;

        ////Writes all of the elements of the array of dialogue texts
        //for (int i = 0; i < target.DialogueText.Length; i++)
        //{
        //    target.DialogueText[i] = EditorGUI.TextField(new Rect(position.xMin, y, position.width, k_SingleLineHeight), target.DialogueText[i]);
        //    y += k_NewLine;
        //}

        target.dialogueTextReorderableList.DoList(new Rect(position.xMin, y, position.width, target.dialogueTextReorderableList.GetHeight()));
        y += target.dialogueTextReorderableList.GetHeight() + k_PaddingBetweenRules;

        //Gets the string of all the methods with parameters "List<Dialogue>"
        List<string> methodStrings = DialogueMethod.DialogueMethods();

        //Creates a dropdown list of the different types of dialogues
        GUI.Label(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Type");
        if (EditorGUI.DropdownButton(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), new GUIContent(target.ThisType.ToString()), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            AddMenuItemForType(menu, "Custom", new object[2] { new Dialogue.ResponsesType() { Type = Dialogue.Type.Custom, ValueOfType = "" }, property });
            menu.AddSeparator("");
            for (int i = 0; i < methodStrings.Count; i++)
            {
                AddMenuItemForType(menu, "Method/" + methodStrings[i], new object[2] { new Dialogue.ResponsesType { Type = Dialogue.Type.Method, ValueOfType = methodStrings[i].Split('/')[1] }, property });
            }
            foreach (ReuseableData list in rootParent.SavedResponses)
            {
                AddMenuItemForType(menu, "Dialogue/" + list.IdName,
                    new object[2] { new Dialogue.ResponsesType() { Type = Dialogue.Type.Dialogue, PossibleResponses = list }, property });
            }
            menu.ShowAsContext();
        }
        y += k_NewLine;


        //EditorGUI.PropertyField(new Rect(position.xMin, y, position.width, k_SingleLineHeight), 
        //    property.FindPropertyRelative("typeMethod"));
        //y += k_NewLine + 75;
        
        height = y;

        //If the dialogue type is not of type loaded dialogue, draw the reorderable list
        if (target.ThisType.Type != Dialogue.Type.Dialogue)
        {
            property.serializedObject.Update();
            target.responseReorderableList.DoList(new Rect(position.xMin, y, position.width, 1000f));
            property.serializedObject.ApplyModifiedProperties();

            height += target.responseReorderableList.GetHeight() + k_PaddingBetweenRules;
        }       
    }

    /// <summary>
    /// For when ever an item is added to the dropdown list of types
    /// </summary>
    /// <param name="menu">A reference to the menu</param>
    /// <param name="menuPath">The path that this menu needs to write to</param>
    /// <param name="type">The object that is at the path</param>
    private void AddMenuItemForType(GenericMenu menu, string menuPath, object type)
    {
        menu.AddItem(new GUIContent(menuPath), target.ThisType.Equals((Dialogue.ResponsesType)((object[])type)[0]), OnTypeSelected, type);
    }

    /// <summary>
    /// Called when an item of the type drop down is selected
    /// </summary>
    /// <param name="type">Consists of the serlialized version of the target and the type</param>
    private void OnTypeSelected(object type)
    {
        Dialogue target = Serialize.GetThis((SerializedProperty)((object[])type)[1]) as Dialogue;
        target.ThisType = (Dialogue.ResponsesType)((object[])type)[0];
    }

    /// <summary>
    /// Initializes the target, parent, and reorderable list
    /// </summary>
    /// <param name="property">The serialized version of the target</param>
    /// <param name="position">The position to write to</param>
    private void Initialize(SerializedProperty property, Rect position)
    {       
        //Gets the target and other math given the serialized property
        this.property = property;       
            target = Serialize.GetThis(property) as Dialogue;
            parent = Serialize.GetParent(property);
        rootParent = Serialize.GetRootParent(property) as DialogueEditor;        

        //Initializes the reorderable list
        if (target.responseReorderableList == null)
        {
            target.responseReorderableList = new ReorderableList(target.PossibleResponses, typeof(Response), true, true, true, true);

            //Called when the reorderable list needs to draw the element
            target.responseReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //Serializes the item in the reorderable list
                SerializedProperty element = this.property.FindPropertyRelative("PossibleResponses").GetArrayElementAtIndex(index);                                              
                EditorGUI.PropertyField(new Rect(rect.xMin, rect.yMin, rect.width, 100f), element);                
            };

            //Draws the header of the reorderable list
            target.responseReorderableList.drawHeaderCallback = (Rect rect) =>
            {
                //Draws the settings icon
                GUIContent content = new GUIContent(EditorGUIUtility.IconContent("SettingsIcon"));
                EditorGUI.LabelField(new Rect(rect.xMin, rect.yMin, rect.width - content.image.width, k_SingleLineHeight), "Responses");
                
                //Creates the dropdown for when the settings icon is pressed
                GUIStyle style = new GUIStyle();                
                if (EditorGUI.DropdownButton(new Rect(rect.xMin + rect.width - content.image.width, rect.yMin + (rect.height - content.image.height)/2, content.image.width, content.image.height), content, FocusType.Passive,style))//new Rect(rect.width - content.image.width, rect.y, content.image.width, content.image.height)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Copy"), false, CopyResponses, this.property);
                    if (rootParent.CopyedResponses == null)
                    {
                        menu.AddDisabledItem(new GUIContent("Paste"));
                    }
                    else
                    {
                        menu.AddItem(new GUIContent("Paste"), false, PastResponses, this.property);
                    }
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add to Saved Responses"), false, AddToSavedResponses, this.property);
                    menu.ShowAsContext();
                }
            };

            //Gets the height of each of the elements in the reorderable list
            target.responseReorderableList.elementHeightCallback = (int index) =>
            {                                                                          
                 return target.PossibleResponses[index].ElementListHeight;                
            };

            //Makes it so the element has no background
            target.responseReorderableList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
            };           
        }

        if (target.dialogueTextReorderableList == null)
        {
            target.dialogueTextReorderableList = new ReorderableList(target.DialogueText, typeof(string), true, true, true, true);

            target.dialogueTextReorderableList.drawHeaderCallback = (Rect rect) => 
            {
                EditorGUI.LabelField(rect, "Dialogue Texts");
            };

            target.dialogueTextReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                target.DialogueText[index] = EditorGUI.TextArea(new Rect(rect.xMin, rect.yMin, rect.width, rect.height), target.DialogueText[index]);                

                //Serializes the item in the reorderable list
                //SerializedProperty element = this.property.FindPropertyRelative("DialogueText").GetArrayElementAtIndex(index);
                //EditorGUI.PropertyField(new Rect(rect.xMin, rect.yMin, rect.width, 100f), element);
            };

            target.dialogueTextReorderableList.onAddCallback = (ReorderableList list) =>
            {
                target.DialogueText.Add("<Enter Dialogue>");                
            };

            //target.dialogueTextReorderableList.elementHeightCallback = (int index) =>
            //{
            //    return target.DialogueText[index].elementHeight;
            //};

            target.dialogueTextReorderableList.elementHeight = k_NewLine;
        }
        //height += target.reorderableList.GetHeight() + dialogueTextsList.GetHeight();  
    }

    /// <summary>
    /// Copies the the reorderable list elements
    /// </summary>
    /// <param name="ThisProperty">The target</param>
    void CopyResponses(object ThisProperty)
    {
        rootParent.CopyedResponses = new List<Response>(((Dialogue)Serialize.GetThis((SerializedProperty)ThisProperty)).PossibleResponses);
    }

    /// <summary>
    /// Pastes the the copyed response in the dialogue editor to the reorderable list
    /// </summary>
    /// <param name="ThisProperty">The target</param>
    void PastResponses(object ThisProperty)
    {
        List<Response> responses = new List<Response>(rootParent.CopyedResponses);
        if (responses != null)
        {
            Dialogue t = ((Dialogue)Serialize.GetThis((SerializedProperty)ThisProperty));
            t.PossibleResponses = responses;
            t.responseReorderableList.list = responses;
        }
        rootParent.CopyedResponses = null;
    }

    /// <summary>
    /// Adds the elements of the reorderable list to the list of saved responses in the dialogue editor
    /// </summary>
    /// <param name="property">The target</param>
    void AddToSavedResponses(object property)
    {        
        //Creates a popup window
        AddToSavedResponsesPopUp window = ScriptableObject.CreateInstance<AddToSavedResponsesPopUp>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        Dialogue target = Serialize.GetThis((SerializedProperty)(property)) as Dialogue;
        //Sets the data needed
        window.SetData(ref target, ref rootParent);
        window.ShowPopup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return height;
    }
}
#endif

