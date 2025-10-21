using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarDisplay : MonoBehaviour
{
    public Image FillImageCover;
    public float changeSpeed;
    public bool BValueChanging;
    public void SetBarValue(float rate)
    {
        //FillImageCover.fillAmount = rate;
        if(_riseValueIE==null)
        {
            _riseValueIE = RiseFillAmount(rate);
            StartCoroutine(_riseValueIE);
        }
        else if(!BValueChanging&& _riseValueIE != null)
        {
            StopCoroutine(_riseValueIE);
            _riseValueIE = RiseFillAmount(rate);
            StartCoroutine(_riseValueIE);
        }
    }
    private IEnumerator _riseValueIE;
    private IEnumerator RiseFillAmount(float targetRate)
    {
        if(FillImageCover.fillAmount<targetRate)
        {
            BValueChanging = true;
            while (FillImageCover.fillAmount<targetRate)
            {
                FillImageCover.fillAmount += Time.deltaTime * changeSpeed;
                yield return null;
            }
        }
        else if(FillImageCover.fillAmount > targetRate)
        {
            BValueChanging = true;
            while (FillImageCover.fillAmount > targetRate)
            {
                FillImageCover.fillAmount -= Time.deltaTime * changeSpeed;
                yield return null;
            }
        }
        else
        {
            
        }
        _riseValueIE = null; 
        BValueChanging = false;
    }
}
