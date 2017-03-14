using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Trigger trigger = (Trigger)target;
        switch (trigger.Type)
        {
            case Trigger.TriggerType.SetHunger:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hunger Options",EditorStyles.boldLabel);
                trigger.hunger = EditorGUILayout.FloatField("Set hunger to", trigger.hunger);
                trigger.relative = EditorGUILayout.Toggle("relative", trigger.relative);
                EditorUtility.SetDirty(trigger);
                break;
        }
    }
}