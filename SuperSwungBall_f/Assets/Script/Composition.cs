using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Composition
{
    /// <summary>
    /// Unity ne supporte pas les Tuples.
    /// Donc, int[] à la place de Tuple<int,int>
    /// </summary>
    private Dictionary<int, int[]> data;
    private string name;
    private string code;

    public Composition(string name_, string code_)
    {
        data = new Dictionary<int, int[]>();
        name = name_;
        code = code_;
    }

    public bool SetPosition(int p, int x, int y)
    {
        if (x < 6 && x >= 0 && y >= 0 && y < 6)
        {
            data[p] = new int[2] { x, y };
            return true;
        }
        Debug.Log("Error for setPosition");
        return false;
    }

    public int[] GetPosition(int p)
    {
        if (data.ContainsKey(p))
            return data[p];
        return new int[2];
    }

    public string Name
    {
        get { return name; }
    }
    public string Code
    {
        get { return code; }
    }
    public Dictionary<int, int[]> Data
    {
        get { return data; }
    }
}
