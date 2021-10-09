using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("Music")]
    [SerializeField] private AudioClip menu;
    [SerializeField] private AudioClip match;
    [SerializeField] private AudioClip timeRunningOut;
    [SerializeField] private AudioClip speedUp;
    [SerializeField] private AudioClip slowDown;
    [SerializeField] private AudioClip mouseMania;
    [SerializeField] private AudioClip catMania;
    [SerializeField] private AudioClip roundOver;
    [SerializeField] private AudioClip gameOver;

    [Header("One-shots")]
    [SerializeField] private AudioClip mouse;
    [SerializeField] private AudioClip explosion;

    private AudioSource _source;

    public void PlayMusic(GameState state)
    {
        var music = state switch
        {
            GameState.Menu => menu,
            GameState.Match => match,
            GameState.TimeRunningOut => timeRunningOut,
            GameState.SpeedUp => speedUp,
            GameState.SlowDown => slowDown,
            GameState.MouseMania => mouseMania,
            GameState.CatMania => catMania,
            GameState.RoundOver => roundOver,
            GameState.GameOver => gameOver,
            _ => null
        };

        if(music)
        {
            _source.clip = music;
            _source.Play();
        }
    }

    public void PlayOneShot(Sound sound)
    {
        var clip = sound switch
        {
            Sound.Mouse => mouse,
            Sound.Explosion => explosion,
            _ => null
        };

        _source.PlayOneShot(clip);
    }

    public enum Sound
    {
        Mouse, Explosion
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
    }
}
