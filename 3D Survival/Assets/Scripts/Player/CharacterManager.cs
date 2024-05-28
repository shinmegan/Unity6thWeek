using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance; // 유일한 인스턴스를 저장할 정적 변수
    public static CharacterManager Instance // 유일한 인스턴스를 반환하는 정적 프로퍼티
    {
        get
        {   // 인스턴스가 없으면 새 게임 오브젝트를 만들어 인스턴스를 추가(방어코드)
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();    
            }
            return _instance;
        }
    }
    
    public Player _player; // 플레이어 캐릭터를 저장하는 변수
    public Player Player // 플레이어 캐릭터를 가져오고 설정하는 프로퍼티
    {
        get { return _player; }
        // 다른 스크립트에서 이 프로퍼티를 통해 플레이어 캐릭터를 접근하고 변경 가능
        set { _player = value; }
    }

    private void Awake()
    {
        if(_instance == null)
        {   // 인스턴스 초기화
            _instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_instance == this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
            }
        }
    }
}
