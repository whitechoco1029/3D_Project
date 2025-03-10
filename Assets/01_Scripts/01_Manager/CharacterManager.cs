using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static CharacterManager _instance;
    public Player _player;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Character").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }
    

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private void Aweke()
    {
        if (_instance = null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this)
            {
                Destroy(gameObject);
            }
        }
    }
}
