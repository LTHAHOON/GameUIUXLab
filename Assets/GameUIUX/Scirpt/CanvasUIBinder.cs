using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CanvasUIBinder))]
public class CanvasUIBindEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty nameHash = serializedObject.FindProperty("_nameHash");
        EditorGUILayout.PropertyField(nameHash);
        if (GUILayout.Button("Generate And Bind ID"))
        {
            CanvasUIBinder binder = (CanvasUIBinder)target;
            binder.Bind();
            EditorUtility.SetDirty(binder);
        }

        serializedObject.ApplyModifiedProperties();
    }

}

public class CanvasUIBinder : MonoBehaviour
{
    [SerializeField]
    private int _nameHash;

    internal void Bind()
    {
        if (!gameObject.transform.root.TryGetComponent(out CanvasDocument canvasDocument))
        {
            Debug.Log($"{gameObject.name} 해당 Root에 CanvasDocument를 추가해주세요.");
        }
        Debug.Log(gameObject.name);
        _nameHash = Animator.StringToHash(gameObject.name);
        canvasDocument.AddCanvasUIBinder(this);
    }

    public int NameHash => _nameHash;
}
