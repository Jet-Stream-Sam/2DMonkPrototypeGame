using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
public class RequireInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            var requiredAttribute = attribute as RequireInterfaceAttribute;

            EditorGUI.BeginProperty(position, label, property);

            Object obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);

            if (obj == null)
                property.objectReferenceValue = null;
            else if (obj is GameObject g) 
                property.objectReferenceValue = g.GetComponent(requiredAttribute.requiredType);
            
            
            EditorGUI.EndProperty();

        }
        else
        {
            var previousColor = GUI.color;
            GUI.color = Color.red;

            EditorGUI.LabelField(position, label, new GUIContent("Property is not a reference."));

            GUI.color = previousColor;
        }
    }
}
