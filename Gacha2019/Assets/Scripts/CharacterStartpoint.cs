using UnityEngine;

public class CharacterStartpoint : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Character.Teleport(m_StartPoint.GameGrid, m_StartPoint.Row, m_StartPoint.Column);
    }

    [SerializeField] private GridCell m_StartPoint = null;
}
