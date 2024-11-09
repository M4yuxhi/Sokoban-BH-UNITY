using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void Start() => LevelReader.LoadLevels();

    void Update()
    {
        bool showLevels = Input.GetKeyDown(KeyCode.L);

        if (showLevels)
            Debug.Log(LevelReader.LevelCount);
    }
}