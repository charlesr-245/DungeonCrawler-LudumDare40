using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicStats : MonoBehaviour {

    private AnimationManager animationManager;
    public bool isPlayer;
    public Text HPText;
    public enum Stats
    {
        Attack,
        Defense,
        Radius,
        HP,
        AttackCooldown
    }

    public float baseAttack, baseDefense, baseRadius, baseHP,baseCooldown;

    private float attack, defense, radius, HP, cooldown;

    private void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        attack = baseAttack;
        defense = baseDefense;
        radius = baseRadius;
        HP = baseHP;
        cooldown = baseCooldown;
        if (isPlayer)
        {
            HPText.text = HP.ToString();
        }
    }

    public float GetAttack()
    {
        return attack;
    }

    public float GetDefense()
    {
        return defense;
    }

    public float GetRadius()
    {
        return radius;
    }

    public float GetHP()
    {
        return HP;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public void SetAttack(float attk)
    {
        attack = attk;
    }

    public void SetDefense(float def)
    {
        defense = def;
    }

    public void SetRadius(float rad)
    {
        radius = rad;
    }

    public void DecreaseHP(float amount)
    {
        HP -= amount;
        if (isPlayer)
        {
            HPText.text = HP.ToString();
        }
    }
    public void SetCooldown(float cool)
    {
        cooldown = cool;
    }
}
