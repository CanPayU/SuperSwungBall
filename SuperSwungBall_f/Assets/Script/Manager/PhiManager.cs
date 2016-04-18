using UnityEngine;
using System.Collections;

public class PhiManager : MonoBehaviour
{
    private static PhiManager instance = new PhiManager();

    private GameObject more_panel;


    void Start()
    {
        instance.more_panel = Resources.Load("Prefabs/Setting/MorePhi") as GameObject;
    }

    public void More()
    {
        GameObject gm = Instantiate(instance.more_panel);
        Transform Canvas = GameObject.FindObjectOfType<Canvas>().transform;
        gm.transform.SetParent(Canvas, false);
    }

    public void Add(int value)
    {
        User.Instance.phi += value;
        HTTP.SetPhi(User.Instance.phi, (success) =>
        {
            Debug.Log(success + " - " + User.Instance.phi);
        });
    }

    /// <summary> Verification et synchronisation de l'achat </summary>
    /// <returns><c>true</c>, if player was bought, <c>false</c> otherwise.</returns>
    public bool BuyPlayer(Player p)
    {
        int myPhi = User.Instance.phi;

        if (myPhi < p.Price || Settings.Instance.Default_player.ContainsKey(p.UID))
            return false;

        HTTP.BuySM(p.UID, (success) =>
        {
            if (!success)
                Notification.Create(NotificationType.Box, "Oups ...", content: "Une erreur est survenue lors de la synchronisation.", force: true);
        });
        return true;
    }

    public static PhiManager I
    {
        get { return instance; }
    }
}
