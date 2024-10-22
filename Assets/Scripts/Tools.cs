using UnityEngine;

static class Tools
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPositio = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        return mouseWorldPositio;
    }

    public static Vector3Int GetMouseWorldIntPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        return new Vector3Int(Mathf.RoundToInt(mouseWorldPosition.x), 0, Mathf.RoundToInt(mouseWorldPosition.z));
    }

    public static Vector3 GetMouseWorldHexPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        float xDivisor = 0.5f;
        float zDivisor = 0.86f;

        float xQuotient = mouseWorldPosition.x / xDivisor;
        float zQuotient = mouseWorldPosition.z / zDivisor;

        float hexX = Mathf.Round(xQuotient) * xDivisor;
        float hexZ = Mathf.Round(zQuotient) * zDivisor;

        return new Vector3(hexX, 0, hexZ);
    }

    public static RaycastHit GetMouseRayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit;
        }
        else return default;
    }

    public static Vector3 GetHexPosition(Vector3 origin)
    {
        float xDivisor = 0.5f;
        float zDivisor = 0.86f;

        float xQuotient = origin.x / xDivisor;
        float zQuotient = origin.z / zDivisor;

        float hexX = Mathf.Round(xQuotient) * xDivisor;
        float hexZ = Mathf.Round(zQuotient) * zDivisor;

        return new Vector3(hexX, 0, hexZ);
    }

    public static bool InGridRange(MapGrid grid, Vector2Int id)
    {
        Vector2 gridSize = grid.GetGridSize();

        if (id.x >= 0 && id.x < gridSize.x &&
            id.y >= 0 && id.y < gridSize.y)
            return true;

        else
            return false;
    }

    public static Vector3 GetMouseHexGridPosition(float xOffset, float zOffset)
    {
        Vector3 mousePosition = GetMouseRayHit().point;
        float x = Mathf.Round(mousePosition.x / xOffset) * xOffset;
        float y = 0;
        float z = Mathf.Round(mousePosition.z / zOffset) * zOffset;

        return new Vector3(x, y, z);
    }

    public static Vector3 GetVectorHexGridPosition(Vector3 vector, float xOffset, float zOffset)
    {
        float x = Mathf.Round(vector.x / xOffset) * xOffset;
        float y = 0;
        float z = Mathf.Round(vector.z / zOffset) * zOffset;

        return new Vector3(x, y, z);
    }

    public static void Log(string message)
    {
        Debug.Log(message);
    }
}
