using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数据访问对象模式
/// </summary>
public class DataAccessObjectPattern : MonoBehaviour
{
    public class Student
    {
        public string Name;
        public int RollNo;

        private Student(string name, int rollNo)
        {
            Name = name;
            RollNo = rollNo;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public int GetRollNo()
        {
            return RollNo;
        }

        public void SetRollNo(int rollNo)
        {
            RollNo = rollNo;
        }
    }

    public interface IStudentDao
    {
        List<Student> GetAllStudents();
        Student GetStudent(int rollNo);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
    }

    public class StudentDaoImpl : IStudentDao
    {
        //列表是当作一个数据库
        private List<Student> _students;

        public void DeleteStudent(Student student)
        {
            _students.Remove(student);
        }

        public List<Student> GetAllStudents()
        {
            return _students;
        }

        public Student GetStudent(int rollNo)
        {
            return _students.Find(item => item.RollNo == rollNo);
        }

        public void UpdateStudent(Student student)
        {
            var temp = GetStudent(student.GetRollNo());
            if (temp != null)
            {
                student.SetName(student.Name);
            }
        }
    }

    public void Main()
    {
        IStudentDao studentDao = new StudentDaoImpl();
        //输出所有的学生
        for (int i = 0; i < studentDao.GetAllStudents().Count; i++)
        {
            Debug.Log("student rollNo:" + studentDao.GetAllStudents()[i].GetRollNo()
                + "student name:" + studentDao.GetAllStudents()[i].GetName());
        }
        //更新学生
        Student student = studentDao.GetAllStudents()[0];
        student.SetName("Michael");
        studentDao.UpdateStudent(student);
        //获取学生
        student = studentDao.GetStudent(0);
        Debug.Log("student rollNo:" + student.GetRollNo()
                + "student name:" + student.GetName());
    }
}
