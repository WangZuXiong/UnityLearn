using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组合模式
/// </summary>
public class CompositePattern : MonoBehaviour
{
    public class Employee
    {
        private string _name;
        private string _dept;
        private int _salary;
        private List<Employee> _subordinates;

        public Employee(string name, string dept, int salary)
        {
            _name = name;
            _dept = dept;
            _salary = salary;
            _subordinates = new List<Employee>();
        }

        public void Add(Employee employee)
        {
            _subordinates.Add(employee);
        }

        public void Remove(Employee employee)
        {
            _subordinates.Remove(employee);
        }

        public List<Employee> GetSubordinates()
        {
            return _subordinates;
        }

        public override string ToString()
        {
            return "Employee name" + _name + " Dept:" + _dept + " Salary:" + _salary;
        }
    }


    public void Main()
    {
        Employee CEO = new Employee("John", "CEO", 30000);

        Employee headSales = new Employee("Robert", "Head Sales", 20000);

        Employee headMarketing = new Employee("Michel", "Head Marketing", 20000);

        Employee clerk1 = new Employee("Laura", "Marketing", 10000);
        Employee clerk2 = new Employee("Bob", "Marketing", 10000);

        Employee salesExecutive1 = new Employee("Richard", "Sales", 10000);
        Employee salesExecutive2 = new Employee("Rob", "Sales", 10000);

        CEO.Add(headSales);
        CEO.Add(headMarketing);

        headSales.Add(salesExecutive1);
        headSales.Add(salesExecutive2);

        headMarketing.Add(clerk1);
        headMarketing.Add(clerk2);

        for (int i = 0; i < CEO.GetSubordinates().Count; i++)
        {
            for (int j = 0; j < CEO.GetSubordinates()[i].GetSubordinates().Count; j++)
            {
                Debug.Log(CEO.GetSubordinates()[i].GetSubordinates()[i].ToString());
            }
        }
    }
}
