using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class LevelReader
{
    private static List<Level> levels = new List<Level>();

    enum DirectoryNames
    {
        Levels
    }

    public static int LevelCount => levels.Count;
    
    public static List<Level> Levels => levels;

    public static void LoadLevels()
    {
        string directory = DirectoryNames.Levels.ToString();
        DirectoryHelper.CheckDirectory(directory);

        string path = Path.Combine(Application.dataPath, directory);
        string[] levelFiles = Directory.GetFiles(path, "*.txt");

        foreach (var file in levelFiles) 
        {
            Debug.Log("Cargando nivel desde archivo: " + file);

            string fileContent = ReadLevel(file);
            ProcessLevelData(fileContent);
        }
    }

    private static string ReadLevel(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        using(StreamReader sr = new StreamReader(filePath))
        {
            string line;

            while((line = sr.ReadLine()) != null)
                sb.AppendLine(line);
        }

        return sb.ToString();
    }

    private static void ProcessLevelData(string data)
    {
        Debug.Log("Contenido del nivel: " +  data);

        string[] lines = data.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        List<Vector2> boxesInitialPos = new();
        List<Vector2> floorGridsPos = new();
        List<Vector2> goalsPos = new();
        List<Vector2> wallsPos = new();

        Vector2 playerInitialPos = new();
        Vector2 size = new Vector2();
        for (int row = 0; row < lines.Length; row++) 
        {
            string[] cells = lines[row].Trim().Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            size.y = lines.Length;
            for (int col = 0; col < cells.Length; col++)
            {
                int value;
                size.x = cells.Length;
                // Verificar si el valor es numérico y convertirlo
                if (!int.TryParse(cells[col], out value))
                {
                    Debug.LogError($"Valor no numérico encontrado en la línea {row + 1}, columna {col + 1}: {cells[col]}");
                    continue;  // Ignorar el valor si no es numérico
                }

                float y = lines.Length - row;
                Vector2 position = new(col, y);

                switch (value)
                {
                    case 0 : floorGridsPos.Add(position);
                        break;
                    case 1 : wallsPos.Add(position);
                        break;
                    case 2 : 
                        playerInitialPos = position;
                        floorGridsPos.Add(position);
                        break;
                    case 3 : 
                        boxesInitialPos.Add(position);
                        floorGridsPos.Add(position);
                        break;
                    case 4 : goalsPos.Add(position);
                        break;
                }
            }
        }

        Debug.Log("Boxes: " + boxesInitialPos.Count + ", Floor: " + floorGridsPos.Count + ", Goals: " + goalsPos.Count + ", Walls: " + wallsPos.Count + ", Player: yup");

        Level level = new Level(boxesInitialPos, floorGridsPos, goalsPos, wallsPos, playerInitialPos, size);
        levels.Add(level);
    }
}