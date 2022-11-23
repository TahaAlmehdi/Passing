using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFreeze : MonoBehaviour {




    bool _isForzen = false;


    public void Freeze(float duration) {

        if (_isForzen)
            return;

        StartCoroutine(DoFreeze(duration));

    }

    IEnumerator DoFreeze(float duration)
    {
        _isForzen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        _isForzen = false;
    }
}
