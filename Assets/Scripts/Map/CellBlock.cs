using TMPro;
using UnityEngine;

public class CellBlock : MonoBehaviour
{
    MapCell cell;
    [SerializeField] MeshRenderer visual;
    [SerializeField] Material path;
    [SerializeField] Material normal;
    [SerializeField] TextMeshPro cellText;

    public void SetCell(MapCell cell)
    {
        this.cell = cell;
    }

    public MapCell GetCell()
    {
        return cell;
    }

    public void SetCellText(string text)
    {
        cellText.text = text;
    }

    public void IsPath(bool path)
    {
        if (path)
        {
            visual.material = this.path;
        }
        else
        {
            visual.material = normal;
        }
    }
}
