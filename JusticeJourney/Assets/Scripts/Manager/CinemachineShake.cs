using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    // Singleton pattern: Biến Instance để truy cập từ bất kỳ đâu trong code
    public static CinemachineShake Instance;

    // Tham chiếu đến Cinemachine Virtual Camera
    CinemachineVirtualCamera _cvc;

    // Tham chiếu đến Cinemachine Basic MultiChannel Perlin, một thành phần để tạo hiệu ứng rung
    CinemachineBasicMultiChannelPerlin _cPerlin;

    // Biến để theo dõi thời gian còn lại của hiệu ứng rung
    float _shakeTimer;

    // Biến để lưu tổng thời gian của hiệu ứng rung
    float _shakeTimerTotal;

    // Biến để lưu giá trị ban đầu của độ cường độ rung
    float _startingIntensity;

    // Phương thức Awake được gọi khi đối tượng được khởi tạo
    void Awake()
    {
        // Singleton pattern: Đảm bảo chỉ có một đối tượng CinemachineShake tồn tại
        Instance = this;

        // Lấy tham chiếu đến Cinemachine Virtual Camera và Cinemachine Basic MultiChannel Perlin
        _cvc = GetComponent<CinemachineVirtualCamera>();
        _cPerlin = _cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Phương thức để kích hoạt hiệu ứng rung của camera
    public void ShakeCamera(float intensity, float time)
    {
        // Thiết lập cường độ rung cho Cinemachine Basic MultiChannel Perlin
        _cPerlin.m_AmplitudeGain = intensity;

        // Lưu giữ giá trị ban đầu và thời gian của hiệu ứng rung
        _startingIntensity = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
    }

    // Phương thức Update được gọi mỗi frame
    void Update()
    {
        // Kiểm tra nếu hiệu ứng rung đang diễn ra
        if (_shakeTimer > 0)
        {
            // Giảm thời gian còn lại của hiệu ứng rung
            _shakeTimer -= Time.deltaTime;

            // Nếu thời gian còn lại là 0, đặt cường độ rung về 0
            if (_shakeTimer <= 0f)
            {
                _cPerlin.m_AmplitudeGain = 0f;

                // Lerp giữa giá trị ban đầu và 0 để tạo hiệu ứng mượt mà khi kết thúc hiệu ứng rung
                Mathf.Lerp(_startingIntensity, 0f, 1f - (_shakeTimer / _shakeTimerTotal));
            }
        }
    }
}
