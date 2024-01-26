using System.Collections.Generic;
using UnityEngine;

// Lớp trừu tượng GenericObjectPool quản lý một pool các đối tượng tái sử dụng
// Kiểu T phải là một Component
public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] T _prefab;           // Đối tượng được sử dụng để tạo các đối tượng trong pool
    [SerializeField] int _initialPoolSize; // Kích thước ban đầu của pool
    Queue<T> _objectsQueue = new Queue<T>(); // Hàng đợi chứa các đối tượng trong pool

    // Singleton pattern: Một instance của pool duy nhất được tạo trong game
    public static GenericObjectPool<T> Instance { get; private set; }

    // Awake được gọi trước Start, đảm bảo chỉ có một instance của pool được tạo
    void Awake()
    {
        Instance = this;
    }

    // Start được gọi khi game bắt đầu, khởi tạo pool với kích thước ban đầu
    void Start()
    {
        AddObjects(_initialPoolSize);
    }

    // Phương thức Get để lấy một đối tượng từ pool
    public T Get()
    {
        // Nếu pool rỗng, thêm thêm đối tượng vào pool
        if (_objectsQueue.Count == 0)
        {
            AddObjects(1);
        }

        // Lấy một đối tượng từ hàng đợi và kích hoạt nó
        T objectToGet = _objectsQueue.Dequeue();
        objectToGet.gameObject.SetActive(true);

        return objectToGet;
    }

    // Phương thức Get với vị trí và hướng để lấy đối tượng và thiết lập vị trí, hướng
    public T Get(Vector3 position, Quaternion rotation)
    {
        // Nếu pool rỗng, thêm thêm đối tượng vào pool
        if (_objectsQueue.Count == 0)
        {
            AddObjects(1);
        }

        // Lấy một đối tượng từ hàng đợi và thiết lập vị trí, hướng, sau đó kích hoạt nó
        T objectToGet = _objectsQueue.Dequeue();
        objectToGet.transform.position = position;
        objectToGet.transform.rotation = rotation;
        objectToGet.gameObject.SetActive(true);

        return objectToGet;
    }

    // Phương thức thêm một số lượng đối tượng mới vào pool
    public void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Tạo một đối tượng mới từ prefab, vô hiệu hóa nó, và thêm vào pool
            T newObject = Instantiate(_prefab);
            newObject.gameObject.SetActive(false);
            _objectsQueue.Enqueue(newObject);
        }
    }

    // Phương thức để đặt một đối tượng trả về vào pool
    public void ReturnToPool(T objectToReturn)
    {
        // Vô hiệu hóa đối tượng và thêm vào pool
        objectToReturn.gameObject.SetActive(false);
        _objectsQueue.Enqueue(objectToReturn);
    }
}
