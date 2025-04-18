using System.Collections.Generic;
using Auth;
using Environments;
using UnityEngine;

public class SessionData : MonoBehaviour
{
    public static TokenDto TokenDto;
    public static string EnvironmentId;
    public static List<Environment2D> Environments = null;
    public const string Url = "https://avansict2238591.azurewebsites.net";
    // public const string Url = "https://localhost:7287";
    
    
    public static SessionData instance { get; private set; }
    void Awake()
    {
        // hier controleren we of er al een instantie is van deze singleton
        // als dit zo is dan hoeven we geen nieuwe aan te maken en verwijderen we deze
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
