#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomPropertyDrawer(typeof(DialogueText))]
public class DialogueTextDrawer : PropertyDrawer
{
    ///<summary>
    ///The height of a single line
    ///</summary>    
    const float k_SingleLineHeight = 16f;

    /// <summary>
    /// The width of a label
    /// </summary>
    const float k_LabelWidth = 105f;

    /// <summary>
    /// The height of a line plus the padding
    /// </summary>
    const float k_NewLine = k_SingleLineHeight + 5f;
    
    /// <summary>
    /// The property being displayed
    /// </summary>
    DialogueText target;

    /// <summary>
    /// The height of this drawer
    /// </summary>
    private float height;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        target = Serialize.GetThis(property) as DialogueText;
        float y = position.yMin + 2;

        target.Message = EditorGUI.TextField(new Rect(position.xMin, y, position.width, k_SingleLineHeight), target.Message);
        y += k_NewLine;

        EditorGUI.LabelField(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Has method");
        target.HasAfterMessage = EditorGUI.Toggle(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), target.HasAfterMessage);
        y += k_NewLine;

        if (target.HasAfterMessage)
        {
            EditorGUI.PropertyField(new Rect(position.xMin, y, position.width, 75), property.FindPropertyRelative("AfterMessage"));
            y += 30 * target.AfterMessage.GetPersistentEventCount() + 75;                        
        }

        EditorGUI.LabelField(new Rect(position.xMin, y, k_LabelWidth, k_SingleLineHeight), "Ends the dialogue");
        target.EndDialogue = EditorGUI.Toggle(new Rect(position.xMin + k_LabelWidth, y, position.width - k_LabelWidth, k_SingleLineHeight), target.EndDialogue);
        y += k_NewLine;

        height = y - position.yMin;
        target.elementHeight = height;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return target.elementHeight;
    }
}

#endif