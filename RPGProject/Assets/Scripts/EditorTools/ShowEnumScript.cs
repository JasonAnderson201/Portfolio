using UnityEngine;
using UnityEditor;
using System;



[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public sealed class ShowEnumAttribute : PropertyAttribute
{
    public string ConditionalSourceField;
    public int expectedValueIndex;
    public bool HideInInspector;

    public ShowEnumAttribute(string ConditionalSourceField, int expectedValueInt, bool HideInInspector = false)
    {
        this.ConditionalSourceField = ConditionalSourceField;
        this.expectedValueIndex = expectedValueInt;
        this.HideInInspector = HideInInspector;
    }
}
#if UNITY_EDITOR
#region enum show
[CustomPropertyDrawer(typeof(ShowEnumAttribute))]
public class ConditionalHideEnumPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowEnumAttribute condHAtt = (ShowEnumAttribute)attribute;
        if (condHAtt != null)
        {
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

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowEnumAttribute condEnum = (ShowEnumAttribute)attribute;

        bool enabled = GetConditionalSourceField(property, condEnum);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            if (!condEnum.HideInInspector)
            {
                return EditorGUI.GetPropertyHeight(property, label, false);
            }

            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalSourceField(SerializedProperty property, ShowEnumAttribute condEnum)
    {
        bool enabled = false;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, condEnum.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.intValue == condEnum.expectedValueIndex;
            if (enabled)
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