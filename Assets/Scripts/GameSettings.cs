using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public enum Character
    {
        Grumpy,
        Rebellious
    }

    public enum GameMode
    {
        Single,
        Duo
    }

    public static Character m_chosenChar;
    public static GameMode m_mode = GameMode.Single;

    public static void SwitchChar(Character character)
    {
        if (m_chosenChar != character)
            m_chosenChar = character;
    }

    public static void SetGameMode(GameMode mode)
    {
        m_mode = mode;
    }
}
