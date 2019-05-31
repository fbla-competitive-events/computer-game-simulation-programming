#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(Response))]
public class ResponseDrawer : PropertyDrawer
{    
    /// <summary>
    /// The height of a line
    /// </summary>
    const float k_SingleLineHeight = 16f;

    /// <summary>
    /// The width of a label
    /// </summary>
    const float k_LabelWidth = 53f;

    /// <summary>
    /// The height of an element
    /// </summary>
    const float k_DefaultElementHeight = 32f;

    /// <summary>
    /// The padding between two elements
    /// </summary>
    const float k_PaddingBetweenRules = 13f;

    /// <summary>
    /// The newline value
    /// </summary>
    const float k_NewLine = k_SingleLineHeight + 5f;

    /// <summary>
    /// The response that this class is serializing
    /// </summary>
    Response target;

    /// <summary>
    /// The dialogue that contains this response in its list of possible responses
    /// </summary>
    Dialogue parent;

    /// <summary>
    /// The Dialogue editor
    /// </summary>
    DialogueEditor rootParent;

    /// <summary>
    /// The serialized version of target
    /// </summary>
    SerializedProperty property;

    /// <summary>
    /// Draws the response
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
                
        Initialize(property, position);
                              
        target.Name = parent.Name;

        //The y value of the different things being displayed
        float y = position.yMin;

        //The Text that the response will show
        GUI.Label(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Text");
        target.Text = EditorGUI.TextField(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), target.Text);
        y += k_NewLine;

        //The type of response it is. Creates a dropdown where the user can choose betwen custom, method, or saved dialogue
        List<string> methodStrings = DialogueMethod.EnterMethods();
        GUI.Label(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Type");
        if (EditorGUI.DropdownButton(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), new GUIContent(target.ThisType.ToString()), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            AddMenuItemForType(menu, "Custom", new object[2] { new Response.PossibleDialoguesType() { Type = Response.Type.Custom, ValueOfType = "" }, property });
            menu.AddSeparator("");
            for (int i = 0; i < methodStrings.Count; i++)
            {
                AddMenuItemForType(menu, "Method/" + methodStrings[i], new object[2] { new Response.PossibleDialoguesType() { Type = Response.Type.Method, ValueOfType = methodStrings[i].Split('/')[1] }, property });
            }
            foreach (ReuseableData list in rootParent.SavedPossibleDialogues)
            {
                AddMenuItemForType(menu, "Dialogue/" + list.IdName,
                    new object[2] { new Response.PossibleDialoguesType() { Type = Response.Type.Dialogue, PossibleDialogues = list }, property });
            }
            foreach (ReuseableData list in rootParent.LoadedDialogues)
            {
                AddMenuItemForType(menu, "Loaded Dialogue/" + list.IdName,
                    new object[2] { new Response.PossibleDialoguesType() { Type = Response.Type.LoadDialogue, PossibleDialogues = list }, property });
            }
            menu.ShowAsContext();
        }
        y += k_NewLine;
        
        //As long as the type is not that of saved dialogue, display the reorderable list of possible dialogues
        if (target.ThisType.Type == Response.Type.Custom || target.ThisType.Type == Response.Type.Method)
        {
            property.serializedObject.Update();
            target.reorderableList.DoList(new Rect(position.xMin, y, position.width, 1000f));
            property.serializedObject.ApplyModifiedProperties();
        }        
    }

    /// <summary>
    /// Creates a menu item for the type dropdwon
    /// </summary>
    /// <param name="menu">The menu to create for</param>
    /// <param name="menuPath">Where to create the item</param>
    /// <param name="type">The object stored at this path</param>
    private void AddMenuItemForType(GenericMenu menu, string menuPath, object type)
    {
        menu.AddItem(new GUIContent(menuPath), target.ThisType.Equals((Response.PossibleDialoguesType)((object[])type)[0]), OnTypeSelected, type);
    }

    /// <summary>
    /// Called when an item on the type dropdown is selected
    /// </summary>
    /// <param name="type">an array containting the target and the type</param>
    private void OnTypeSelected(object type)
    {
        //Gets the target using the serialized object, and then sets the type to it
        Response target = Serialize.GetThis((SerializedProperty)((object[])type)[1]) as Response;
        target.ThisType = (Response.PossibleDialoguesType)((object[])type)[0];
    }

    /// <summary>
    /// Used to initialize the target variable and reorderable list
    /// </summary>
    /// <param name="property">The serialized version of target</param>
    /// <param name="position">Where the reorderable list should go</param>
    private void Initialize(SerializedProperty property, Rect position)
    {
        //Initializes the properites
        this.property = property;        
        target = Serialize.GetThis(property) as Response;
        parent = Serialize.GetParent(property) as Dialogue;
        rootParent = Serialize.GetRootParent(property) as DialogueEditor;
       
        //Creates the reorderable list
        if (target.reorderableList == null)
        {
            target.reorderableList = new ReorderableList(target.PossibleDialogues, typeof(Dialogue), true, true, true, true);

            //Called when needing to draw dialogue element
            target.reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = this.property.FindPropertyRelative("PossibleDialogues").GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.xMin, rect.yMin, rect.width, 100f), element);                
            };

            //Called when drawing the header. Draws a setting icon that allows the user to copy, paste, or save the list of possible dialogues
            target.reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                GUIContent content = new GUIContent(EditorGUIUtility.IconContent("SettingsIcon"));
                EditorGUI.LabelField(new Rect(rect.xMin, rect.yMin, rect.width - content.image.width, k_SingleLineHeight), "Possible Dialogues");

                GUIStyle style = new GUIStyle();

                //Creates the dropdown for the setting icon
                if (EditorGUI.DropdownButton(
                    new Rect(rect.xMin + rect.width - content.image.width, rect.yMin + (rect.height - content.image.height) / 2, content.image.width, content.image.height), 
                    content, FocusType.Passive, style))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Copy"), false, CopyDialogue, this.property);
                    if (rootParent.CopyedPossibleDialogues == null)
                    {
                        menu.AddDisabledItem(new GUIContent("Paste"));
                    }
                    else
                    {
                        menu.AddItem(new GUIContent("Paste"), false, PastDialogue, this.property);
                    }
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add to Saved Dialogues"), false, AddToSavedDialogue, this.property);
                    menu.ShowAsContext();
                }
            };

            //Gets the height of each element
            target.reorderableList.elementHeightCallback = (int index) =>
            {
                return target.PossibleDialogues[index].ElementListHeight;
            };

            //Draw a blank background
            target.reorderableList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
            };            

        }
    }

    /// <summary>
    /// Stores the given dialogue to the editor
    /// </summary>
    /// <param name="target">Dialogue to store</param>
    void CopyDialogue(object target)
    {
        rootParent.CopyedPossibleDialogues = new List<Dialogue>(((Response)Serialize.GetThis((SerializedProperty)target)).PossibleDialogues);
    }

    /// <summary>
    /// Pastes the editor dialogue to the given dialogue
    /// </summary>
    /// <param name="target">Dialogue to be pasted</param>
    void PastDialogue(object target)
    {
        List<Dialogue> dialogues = new List<Dialogue>(rootParent.CopyedPossibleDialogues);
        if (dialogues != null)
        {
            Response t = ((Response)Serialize.GetThis((SerializedProperty)target));
            t.PossibleDialogues = dialogues;
            t.reorderableList.list = dialogues;
        }
        rootParent.CopyedPossibleDialogues = null;
    }

    /// <summary>
    /// Creates a popup window where the user can save a given dialogue
    /// </summary>
    /// <param name="property"></param>
    void AddToSavedDialogue(object property)
    {
        //Serialize.Editor.DialogueEditor.SavedResponses.Add(new )
        AddToSavedDialoguesPopUp window = ScriptableObject.CreateInstance<AddToSavedDialoguesPopUp>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        Response target = Serialize.GetThis((SerializedProperty)(property)) as Response;
        window.SetData(ref target, ref rootParent);
        window.ShowPopup();
    }
}
#endif