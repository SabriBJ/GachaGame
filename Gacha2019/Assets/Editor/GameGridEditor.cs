using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor
{
    #region Private Methods
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GridConfiguration();
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnEnable()
    {
        m_RowsProperty = serializedObject.FindProperty("m_Rows");
        m_ColumnsProperty = serializedObject.FindProperty("m_Columns");
        m_PrefabCellProperty = serializedObject.FindProperty("m_PrefabGridCell");
        m_GridProperty = serializedObject.FindProperty("m_GridCells");
        m_GridTarget = (GameGrid)target;
    }

    private void GridConfiguration()
    {
        GUILayout.TextField("Grid Configurations", EditorStyles.boldLabel);
        RowsField();
        ColumnsField();
        PrefabField();
        CreateAndCleanButtons();
        CellsConfigs();
    }

    private void RowsField()
    {
        int newValue = Mathf.Max(1, m_RowsProperty.intValue);
        m_RowsProperty.intValue = EditorGUILayout.IntField("Rows", newValue, GUILayout.ExpandWidth(false));
    }

    private void ColumnsField()
    {
        int newValue = Mathf.Max(1, m_ColumnsProperty.intValue);
        m_ColumnsProperty.intValue = EditorGUILayout.IntField("Columns", newValue, GUILayout.ExpandWidth(false));
    }

    private void PrefabField()
    {
        EditorGUILayout.ObjectField(m_PrefabCellProperty, GUILayout.ExpandWidth(true));
    }

    private void CreateAndCleanButtons()
    {
        Color defaultColor = GUI.color;
        GUI.color = Color.yellow;

        if (GUILayout.Button("Create"))
        {
            CleanGrid();
            CreateGrid();
        }

        GUI.color = Color.red;
        if (GUILayout.Button("Clear"))
        {
            CleanGrid();
        }
        GUI.color = defaultColor;
    }

    private void CreateGrid()
    {
        int rows = m_RowsProperty.intValue;
        int columns = m_ColumnsProperty.intValue;
        m_GridProperty.arraySize = rows * columns;

        string path = AssetDatabase.GetAssetPath(m_PrefabCellProperty.objectReferenceValue);
        GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int indexGridCell = i * columns + j;
                SerializedProperty sp = m_GridProperty.GetArrayElementAtIndex(indexGridCell);
                GameObject cell = PrefabUtility.InstantiatePrefab(prefab as GameObject) as GameObject;
                cell.name = "GridCell [" + i + ";" + j + "]";
                cell.transform.parent = m_GridTarget.transform;
                cell.transform.localPosition = new Vector3(j, 0, i);
                cell.GetComponent<GridCell>().PlaceIn(m_GridTarget, i, j);
                sp.objectReferenceValue = cell;
            }
        }
    }

    private void CleanGrid()
    {
        int lastIndex = m_GridProperty.arraySize - 1;
        while (lastIndex >= 0)
        {
            if (m_GridProperty.GetArrayElementAtIndex(lastIndex).objectReferenceValue)
            {
                GridCell gridCell = m_GridProperty.GetArrayElementAtIndex(lastIndex).objectReferenceValue as GridCell;
                DestroyImmediate(gridCell.gameObject);
            }
            lastIndex--;
        }
        m_GridProperty.arraySize = 0;
    }

    private void CellsConfigs()
    {
        RefreshCellsConfigs();
    }

    private void RefreshCellsConfigs()
    {
        Color defaultColor = GUI.color;
        GUI.color = Color.cyan;
        if (GUILayout.Button("Refresh Meshs"))
        {
            /* FIRST Method */
            for (int i = 0; i < m_GridProperty.arraySize; i++)
            {
                SerializedProperty gridCellProperty = m_GridProperty.GetArrayElementAtIndex(i);
                SerializedObject gridCellObj = new SerializedObject(gridCellProperty.objectReferenceValue);
                SerializedProperty cellConfigProperty = gridCellObj.FindProperty("m_CellConfig");

                //SerializedProperty relativeProperty = gridCellProperty.FindPropertyRelative("m_CellConfig");
                //Debug.Log("value : " + relativeProperty.objectReferenceValue.ToString());

                ApplyChangesOn(gridCellProperty, cellConfigProperty.objectReferenceValue as GridCellConfig);
            }
        }
        GUI.color = defaultColor;
    }

    private void ApplyChangesOn(SerializedProperty _CellProperty, GridCellConfig _GridCellConfig)
    {
        if (_GridCellConfig != null)
        {
            SerializedObject configPropertyObj = new SerializedObject(_GridCellConfig);

            SerializedProperty configMesh = configPropertyObj.FindProperty("m_CellMesh");
            SerializedProperty configMaterial = configPropertyObj.FindProperty("m_CellMaterial");
            SerializedProperty configCharacterCrossable = configPropertyObj.FindProperty("m_CharacterCrossable");
            SerializedProperty configEnemyCrossable = configPropertyObj.FindProperty("m_EnemyCrossable");

            GridCell gridCell = _CellProperty.objectReferenceValue as GridCell;
            gridCell.GetComponent<MeshFilter>().mesh = configMesh.objectReferenceValue as Mesh;
            gridCell.GetComponent<MeshRenderer>().material = configMaterial.objectReferenceValue as Material;
            gridCell.IsCharacterCrossable = configCharacterCrossable.boolValue;
            gridCell.IsEnemyCrossable = configEnemyCrossable.boolValue;
            //(_CellProperty.objectReferenceValue as GridCell).IsCharacterCrossable = configCharacterCrossable.boolValue;
            //(_CellProperty.objectReferenceValue as GridCell).IsEnemyCrossable = configCharacterCrossable.boolValue;
            EditorUtility.SetDirty(gridCell.GetComponent<GridCell>());
            EditorUtility.SetDirty(gridCell);

        }
    }
    #endregion

    #region Attributes
    private SerializedProperty m_RowsProperty = null;
    private SerializedProperty m_ColumnsProperty = null;
    private SerializedProperty m_PrefabCellProperty = null;
    private SerializedProperty m_GridProperty = null;
    private GameGrid m_GridTarget = null;
    #endregion
}
