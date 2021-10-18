using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zombieUIController : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider brainbar;

    private int maxHealth = 100;
    private int maxBrain = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        healthbar.maxValue = maxHealth;
        brainbar.maxValue = maxBrain;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getHealth
    {
        get { return healthbar.value; }
    }
    
    public void setHealth(int newHealth)
    {
        healthbar.value += newHealth;
    }

    public float getBrain
    {
        get { return brainbar.value; }
    }

    public void setBrain(int newBrain)
    {
        brainbar.value += newBrain;
    }
}
