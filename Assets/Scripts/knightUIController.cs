using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class knightUIController : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider brainbar;
    
    private int maxHealth = 100;
    [SerializeField] private int maxBrain = 100;
    void Start()
    {
        healthbar.maxValue = maxHealth;
        brainbar.maxValue = maxBrain;
    }
    
    void Update()
    {
        
    }

    public float getHealth()
    {
        return healthbar.value;
    }
    public void setHealth(int newHealth)
    {
        healthbar.value += newHealth;
    }

    public float getBrain()
    {
        return brainbar.value;
    }

    public void setBrain(int newBrain)
    {
        brainbar.value += newBrain;
    }
}
