using System;

public static class GameEvents
{
    public static Action OnGamePaused;   // Oyun durduğunda tetiklenecek event
    public static Action OnGameResumed;  // Oyun devam ettiğinde tetiklenecek event
    
    // EXPERIENCE EVENTLERI
    public static Action<int> OnExperienceGathered;  // Exp toplandı
    public static Action OnLevelUp; // level atlandı
    public static Action<int, int> OnExperienceUpdated; // currentXP, maxXP

    // HEALTH EVENTLERI
    public static Action<float, float> OnHealthChanged;
    public static Action OnZeroHealth;

}