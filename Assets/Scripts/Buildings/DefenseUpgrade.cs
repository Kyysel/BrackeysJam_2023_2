using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseUpgrade : Upgrade
{
    // shield based defense -- max shield and shield regen are the 2 stats
    
    public int maxShield = 3;
    public float currentShield;
    public float shieldRegen = 1f;
    
    public ShieldBar shieldBar;

    private void Start()
    {
        currentShield = maxShield;
        shieldBar.SetMaxValue(maxShield);
        StartCoroutine(RegenerateShield());
    }

    #region testing ONLY
    private void Update()
    {
        // if we press the spacebar, we take 2 damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(2);
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;
            shieldBar.SetValue(currentShield);
        }
    }
    #endregion

    private IEnumerator RegenerateShield()
    {
        // Automitically updates when the shield regen is changed
        WaitForSeconds waitTime = new WaitForSeconds(1/shieldRegen);
        while (true)
        {
            if (currentShield < maxShield)
            {
                currentShield += 1;
                shieldBar.SetValue(currentShield);
            }
            yield return waitTime;
        }
    }
    
    public override void UpdateStats()
    {
        maxShield += 1;
        shieldRegen += 0.1f;
        shieldBar.SetMaxValue(maxShield);
        StopAllCoroutines();
        StartCoroutine(RegenerateShield());
    }
}
