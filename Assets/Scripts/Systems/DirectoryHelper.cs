using System.IO;
using UnityEngine;

public static class DirectoryHelper
{
    public static void CheckDirectory(string directoryName)
    {
        string directoryPath = Application.dataPath + "/" + directoryName;

        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(fullPath);
    }
}