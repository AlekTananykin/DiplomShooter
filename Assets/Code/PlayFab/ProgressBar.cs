using System;
using System.Collections;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public Action OnProgressFinish;

    public void SetFastFinish() 
    {
        _isHyperFunction = false;
    }

    public void StartProgress() 
    {
        _isHyperFunction = true;
        _time = 0.0f;

        StartCoroutine(ChangeScalePrgress());
    }

    private IEnumerator ChangeScalePrgress()
    {
        float progressScale = 0.0f;
        while (true)
        {
            if (_isHyperFunction)
            {
                progressScale = HyperScale(progressDeltaTime);
            }
            else
            {
                progressScale = LinearScale(progressDeltaTime);
                if (progressScale >= 1.0f)
                {
                    OnProgressFinish?.Invoke();
                    break;
                }
            }

            _targetBar.transform.localScale = new Vector3(progressScale, 1.0f, 1.0f);

            yield return new WaitForSeconds(progressDeltaTime);
        }
    }

    private float LinearScale(float delta)
    {
        _time += delta;

        float linearFunction = _time * _linearSpeed;

        if (linearFunction > 1.0f)
            return 1.0f;

        return linearFunction;
    }

    private float HyperScale(float delta)
    {
        _time += delta;

        return 1.0f - 1.0f / (_time * _hyperSpeed);
    }

    private const float progressDeltaTime = 1.0f;
    private float _time = 0;

    private bool _isHyperFunction = true;

    [SerializeField]
    private float _linearSpeed = 3.0f;

    [SerializeField]
    private float _hyperSpeed = 0.3f;

    [SerializeField]
    private GameObject _targetBar;

}
