using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pattern.stratagy
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            //税收计算器
            TaxCalculate taxCalculate = new TaxCalculate();

            //个人
            PersonTax personTax = new PersonTax(60000);
            taxCalculate.Calculate(personTax);

            //公司
            CompanyTax companyTax = new CompanyTax(100000);
            taxCalculate.Calculate(companyTax);
        }
    }
}