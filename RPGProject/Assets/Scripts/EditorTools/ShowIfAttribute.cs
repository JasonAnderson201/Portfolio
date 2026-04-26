using UnityEngine;
using UnityEditor;
using System;


[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public sealed class ShowIfAttribute : PropertyAttribute
{
    public string ConditionalSourceField;
    public bool expectedValue;
    public bool HideInInspector;

    public ShowIfAttribute(string ConditionalSourceField, bool expectedValue, bool HideInInspector = false)
    {
        this.ConditionalSourceField = ConditionalSourceField;
        this.expectedValue = expectedValue;
        this.HideInInspector = HideInInspector;
    }
}

#if UNITY_EDITOR
#region boolshow
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;

        bool enabled = GetConditionalSourceField(property, condHAtt);
        GUI.enabled = enabled;

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        else if (!condHAtt.HideInInspector)
        {
            EditorGUI.PropertyField(position, property, label, false);
        }
        else
            return;



    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;


        bool enabled = GetConditionalSourceField(property, condHAtt);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            if (!condHAtt.HideInInspector)
            {
                return EditorGUI.GetPropertyHeight(property, label, false);
            }

            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalSourceField(SerializedProperty property, ShowIfAttribute condHAtt)
    {
        bool enabled = false;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if(sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
            if (enabled == condHAtt.expectedValue)
                enabled = true;
            else
            {
                enabled = false;
            }
        }
        else
        {
            string warning = "Something broke, original message was in spanish so good luck";
            Debug.LogWarning(warning);
        }

        return enabled;
    }

}
#endregion

#endif