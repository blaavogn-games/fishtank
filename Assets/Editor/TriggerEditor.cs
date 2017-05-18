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
            /*case Trigger.TriggerType.SetHunger:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hunger Options",EditorStyles.boldLabel);
                trigger.hunger = EditorGUILayout.FloatField("Set hunger to", trigger.hunger);
                trigger.relative = EditorGUILayout.Toggle("relative", trigger.relative);
                EditorUtility.SetDirty(trigger);
                break;*/
           /* case Trigger.TriggerType.TriggerByOther:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Options",EditorStyles.boldLabel);
                trigger.triggerObject = (GameObject) EditorGUILayout.ObjectField("Trigger Object", trigger.triggerObject,typeof(GameObject),false);
                trigger.scriptObject = (GameObject)EditorGUILayout.ObjectField("Object To Instantiate", trigger.scriptObject, typeof(GameObject), false);
                trigger.activateGameObject = EditorGUILayout.Toggle("Activate GameObject", trigger.activateGameObject);
                break;*/
        }
    }
}