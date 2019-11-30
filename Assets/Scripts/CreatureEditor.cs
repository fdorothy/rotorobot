using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Creature))]
public class CreatureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Creature creature = (Creature)target;
        base.DrawDefaultInspector();
        if (creature)
        {
            if (GUILayout.Button("Hit"))
            {
                creature.Hit(1, 10.0f * Vector2.up);
            }
        }
    }
}
