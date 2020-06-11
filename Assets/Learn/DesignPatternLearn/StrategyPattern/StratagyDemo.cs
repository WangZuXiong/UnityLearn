using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 税收计算器
/// </summary>
public class TaxCalculate
{
    public void Calculate(BaseTax tax)
    {
        tax.Calculate();
    }
}

//税收基类
public class BaseTax
{
    public virtual void Calculate()
    {

    }
}
//个人税收
public class PersonTax : BaseTax
{
    private float _tax;

    public PersonTax(float tax)
    {
        this._tax = tax;
    }


    public override void Calculate()
    {
        Debug.Log("需缴税：" + _tax * 0.18f);
    }
}
//公司税收
public class CompanyTax : BaseTax
{
    private float _tax;

    public CompanyTax(float tax)
    {
        this._tax = tax;
    }


    public override void Calculate()
    {
        Debug.Log("需缴税：" + _tax * 0.2f);
    }
}