using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
/// <summary>
/// 传输对象模式
/// </summary>
public class TransferObjectPattern : MonoBehaviour
{
    public class StudentVO
    {
        private string _name;
        private int _rollNo;

        public StudentVO(string name, int rollNo)
        {
            _name = name;
            _rollNo = rollNo;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public int GetRollNo()
        {
            return _rollNo;
        }

        public void SetRollNo(int rollNo)
        {
            _rollNo = rollNo;
        }
    }


    public class StudentBO
    {
        //列表是当作一个数据库
        private List<StudentVO> _students;

        public StudentBO()
        {
            _students = new List<StudentVO>();
            StudentVO student1 = new StudentVO("Robert", 0);
            StudentVO student2 = new StudentVO("John", 1);
            _students.Add(student1);
            _students.Add(student2);
        }

        public void DeleteStudent(StudentVO student)
        {
            _students.Remove(student);
            Debug.Log(student.GetName() + "--" + student.GetRollNo());
        }

        public List<StudentVO> GetAllStudent()
        {
            return _students;
        }

        public StudentVO GetStudent(int rollNo)
        {
            return _students.Find(item => item.GetRollNo().Equals(rollNo));
        }

        public void UpdateStudent(StudentVO studentVO)
        {
            var temp = GetStudent(studentVO.GetRollNo());
            if (temp != null)
            {
                temp.SetName(studentVO.GetName());
            }
        }
    }

    public void Main()
    {
        StudentBO studentBusinessObject = new StudentBO();

        //输出所有的学生
        for (int i = 0; i < studentBusinessObject.GetAllStudent().Count; i++)
        {
            Debug.Log("Name:" + studentBusinessObject.GetAllStudent()[i].GetName()
                + "RollNo:" + studentBusinessObject.GetAllStudent()[i].GetRollNo());
        }
        //更新学生
        StudentVO student = studentBusinessObject.GetAllStudent()[0];
        student.SetName("Michael");
        studentBusinessObject.UpdateStudent(student);

        //获取学生
        student = studentBusinessObject.GetStudent(0);
        Debug.Log("Name:" + student.GetName()
                + "RollNo:" + student.GetRollNo());
    }
}
