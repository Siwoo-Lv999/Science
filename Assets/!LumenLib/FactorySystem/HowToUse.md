Factory System 사용 방법

1. IFactory를 구현하는 클래스를 만드세요.
2. TProduct와 TRequest를 정의하세요.
3. 필요한 Prefab 등을 가져오세요. (직렬화 등으로)
4. Awake()에서 Factories.Register<타입, 타입>(this)로 등록하세요.
5. public 타입 Create(타입 매개변수명)을 정의하세요.
6. OnDestroy()에서 Facotries.UnRegister<타입, 타입>()으로 등록을 해제하세요.

Create 구현 예시

[SerializeField] private GameObject _prefab;

public GameObject Create(Vector3 rec)
{
    var ins = Instantiate(_prefab, rec, Quaternion.identity);
    return ins.gameObject;
}