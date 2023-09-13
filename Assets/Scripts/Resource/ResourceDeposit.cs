using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDeposit : MonoBehaviour
{

    public string resourceName;
    public int maxAmount;
    public int currentAmount;
    public float minDepth;
    public float maxDepth;
    [Range(0, 1)] public float minSize;
    //public float maxSize;
    public float probability;
    public Sprite[] spriteOptions;
    public ParticleSystem collectParticleSystem;
    public BasicSound collectSound;

    private Transform visuals;
    
    void Start()
    {
        currentAmount = maxAmount;
        visuals = gameObject.transform.GetChild(0);
        visuals.Rotate(0, 0, Random.Range(0, 360));

        SpriteRenderer sptRend = visuals.GetComponent<SpriteRenderer>();
        sptRend.sprite = spriteOptions[Random.Range(0, spriteOptions.Length - 1)];

    }

    public void CollectResource(int amount)
    {

        ResourceManager.Instance.ChangeResource(resourceName, amount);
        currentAmount -= amount;
        PlayResourceCollectionFX();

    }

    private void PlayResourceCollectionFX()
    {
        ParticleSystem aniObject = Instantiate(collectParticleSystem, transform.position, collectParticleSystem.transform.rotation);
        Destroy(aniObject.gameObject, collectParticleSystem.main.duration);
        AudioManager.instance.PlaySoundBaseAtPos(collectSound, transform.position, gameObject.name);

        if (currentAmount <= 0)
        {
            AudioManager.instance.PlaySoundBaseAtPos(ResourceManager.Instance.resourceDestroySound, transform.position, gameObject.name);
            Destroy(gameObject);
            return;
        }

        float ratio = Mathf.Lerp(minSize, 1, (float)currentAmount / (float)maxAmount);
        visuals.localScale = new Vector3(ratio, ratio, 0);
        collectSound.soundSettings.pitchRandomRange += new Vector2(0.05f, 0.05f);

    }

}
