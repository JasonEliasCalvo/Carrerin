using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableOptions))]
public class InteractableOptionsEditor : Editor
{
    private SerializedProperty interactionTypesProp;
    private SerializedProperty eventProp;
    private SerializedProperty itemTypeProp;
    private SerializedProperty useOnPickupProp;

    private void OnEnable()
    {
        interactionTypesProp = serializedObject.FindProperty("interactionTypes");
        eventProp = serializedObject.FindProperty("onInteract");
        itemTypeProp = serializedObject.FindProperty("itemType");
        useOnPickupProp = serializedObject.FindProperty("useOnPickup");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(interactionTypesProp, new GUIContent("Seleccionar opción"));

        for (int i = 0; i < interactionTypesProp.arraySize; i++)
        {
            SerializedProperty element = interactionTypesProp.GetArrayElementAtIndex(i);
            InteractionType type = (InteractionType)element.enumValueIndex;

            switch (type)
            {
                case InteractionType.InvokeEvent:
                    EditorGUILayout.PropertyField(eventProp, new GUIContent("Event"));
                    break;
                case InteractionType.Upgrade:

                    break;
                case InteractionType.Degrade:

                    break;
                case InteractionType.Use:
                    EditorGUILayout.PropertyField(itemTypeProp, new GUIContent("Tipo de Ítem"));
                    EditorGUILayout.PropertyField(useOnPickupProp, new GUIContent("Usar al recoger"));
                    break;
            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("justOneInteraction"), new GUIContent("Solo una interacción"));
        serializedObject.ApplyModifiedProperties();
    }
}
