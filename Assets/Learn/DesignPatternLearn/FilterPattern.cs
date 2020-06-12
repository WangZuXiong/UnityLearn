using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 过滤器模式
/// </summary>
public class FilterPattern
{
    public class Person
    {
        private string _name;
        private string _gender;
        private string _maritalStatus;

        public Person(string name, string gender, string maritalStatus)
        {
            _name = name;
            _gender = gender;
            _maritalStatus = maritalStatus;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetGender()
        {
            return _gender;
        }

        public string GetMaritalStatus()
        {
            return _maritalStatus;
        }
    }

    public interface ICriteria
    {
        List<Person> MeetCriteria(List<Person> persons);
    }

    public class CriteriaMale : ICriteria
    {
        public List<Person> MeetCriteria(List<Person> persons)
        {
            List<Person> malePersons = new List<Person>();
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].GetGender().Equals("MALE"))
                {
                    malePersons.Add(persons[i]);
                }
            }
            return malePersons;
        }
    }

    public class CriteriaFemale : ICriteria
    {
        public List<Person> MeetCriteria(List<Person> persons)
        {
            List<Person> femalePersons = new List<Person>();
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].GetGender().Equals("FAMALE"))
                {
                    femalePersons.Add(persons[i]);
                }
            }
            return femalePersons;
        }
    }

    public class CriteriaSingle : ICriteria
    {
        public List<Person> MeetCriteria(List<Person> persons)
        {
            List<Person> siglePersons = new List<Person>();
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].GetGender().Equals("SIGLE"))
                {
                    siglePersons.Add(persons[i]);
                }
            }
            return siglePersons;
        }
    }

    public class AndCriteria : ICriteria
    {
        private ICriteria _criteria;
        private ICriteria _otherCriteria;

        public AndCriteria(ICriteria criteria, ICriteria otherCriteria)
        {
            _criteria = criteria;
            _otherCriteria = otherCriteria;
        }
        public List<Person> MeetCriteria(List<Person> persons)
        {
            List<Person> firstCriteriaPersons = _criteria.MeetCriteria(persons);
            return _otherCriteria.MeetCriteria(firstCriteriaPersons);
        }
    }

    public class OrCriteria : ICriteria
    {
        private ICriteria _criteria;
        private ICriteria _otherCriteria;

        public OrCriteria(ICriteria criteria, ICriteria otherCriteria)
        {
            _criteria = criteria;
            _otherCriteria = otherCriteria;
        }
        public List<Person> MeetCriteria(List<Person> persons)
        {
            List<Person> firstCriteriaPersons = _criteria.MeetCriteria(persons);
            List<Person> otherCriteriaPersons = _otherCriteria.MeetCriteria(persons);

            for (int i = 0; i < otherCriteriaPersons.Count; i++)
            {
                if (!firstCriteriaPersons.Contains(otherCriteriaPersons[i]))
                {
                    firstCriteriaPersons.Add(otherCriteriaPersons[i]);
                }
            }
            return firstCriteriaPersons;
        }
    }

    public void Main()
    {
        List<Person> persons = new List<Person>();
        persons.Add(new Person("Robert", "Male", "Single"));
        persons.Add(new Person("John", "Male", "Married"));
        persons.Add(new Person("Laura", "Female", "Married"));
        persons.Add(new Person("Diana", "Female", "Single"));
        persons.Add(new Person("Mike", "Male", "Single"));
        persons.Add(new Person("Bobby", "Male", "Single"));

        ICriteria male = new CriteriaMale();
        ICriteria female = new CriteriaFemale();
        ICriteria single = new CriteriaSingle();
        ICriteria singleMale = new AndCriteria(single, male);
        ICriteria singleOrFemale = new OrCriteria(single, female);


        PrintPersons(male.MeetCriteria(persons));
        PrintPersons(female.MeetCriteria(persons));
        PrintPersons(single.MeetCriteria(persons));
        PrintPersons(singleMale.MeetCriteria(persons));
        PrintPersons(singleOrFemale.MeetCriteria(persons));
    }


    public void PrintPersons(List<Person> persons)
    {
        for (int i = 0; i < persons.Count; i++)
        {
            Debug.Log("Name:" + persons[i].GetName() + "Gender:" + persons[i].GetGender() + "Marital Status:" + persons[i].GetMaritalStatus());
        }
    }
}
