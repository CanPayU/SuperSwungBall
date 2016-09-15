using System.Collections.Generic;
using Boomlagoon.JSON;

using GameScene.Instantiator;
using Network;

public static class ApplicationModel
{

    public static JSONArray ChallengeCompleted = null;
    /// <summary>
    /// Type de controller a instancier au lancement de la game
    /// </summary>
    public static GameType TypeToInstanciate;
    /// <summary>
    /// Statut de la game à lancer
    /// </summary>
    public static NetSate NetState;
    /// <summary>
    /// Room Id at join.
    /// </summary>
    public static string RoomID;
    /// <summary>
    /// Nom du fichier si on lance un replay
    /// </summary>
    public static string replayName;
    /// <summary>
    /// La musique est elle en cours ?
    /// </summary>
    public static bool tetomaIsPlaying = false;


    private static int subviewCount = 0;

    public static void AddSubview() { subviewCount++; }
    public static void RemoveSubview() { subviewCount--; }

    public static bool BackgroundSceneActionAllowed
    {
        get { return subviewCount == 0; }
    }
}