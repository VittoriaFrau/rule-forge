using UnityEngine;

[ExecuteInEditMode]
public class GlobalPositionDisplay : MonoBehaviour
{
    // Imposta il valore iniziale delle coordinate globali con la posizione corrente dell'oggetto
    public Vector3 globalPosition = Vector3.zero;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            // Aggiorna le coordinate globali solo durante l'editing
            globalPosition = transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(globalPosition, 0.1f);
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(GlobalPositionDisplay))]
public class GlobalPositionDisplayEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        GlobalPositionDisplay globalPositionDisplay = (GlobalPositionDisplay)target;

        DrawDefaultInspector();

        // Mostra e consente la modifica delle coordinate globali nell'Editor GUI
        globalPositionDisplay.globalPosition = UnityEditor.EditorGUILayout.Vector3Field("Global Position", globalPositionDisplay.globalPosition);
        if (GUILayout.Button("Move Object"))
        {
            globalPositionDisplay.transform.position = globalPositionDisplay.globalPosition;
        }
    }
}
#endif