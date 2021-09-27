using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTeleportation : MonoBehaviour
{
	[SerializeField] private GridCell m_CellDestination = null;


	public void Teleportation()
	{
		GameManager.Instance.Character.Teleport(m_CellDestination.GameGrid, m_CellDestination.Row, m_CellDestination.Column);
	}
}
