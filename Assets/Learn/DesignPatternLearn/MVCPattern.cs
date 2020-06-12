using UnityEngine;
/// <summary>
/// MVC 模式
/// </summary>
public class MVCPattern
{
    public class Student
    {
        private string _rollNo;
        private string _name;

        public string GetRollNo()
        {
            return _rollNo;
        }

        public void SetRollNo(string rollNo)
        {
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
    }
    //创建视图。
    public class StudentView
    {
        public void PrintStudentDetails(string studentName, string studentRollNo)
        {
            Debug.Log("Student:");
            Debug.Log("StudentName:" + studentName);
            Debug.Log("StudentRollNo:" + studentRollNo);
        }
    }
    //创建控制器。
    public class StudentController
    {
        public Student _model;
        public StudentView _view;

        public StudentController(Student model, StudentView view)
        {
            _model = model;
            _view = view;
        }

        public void SetStudentName(string name)
        {
            _model.SetName(name);
        }

        public string GetStudentName()
        {
            return _model.GetName();
        }

        public void SetStudentRollNo(string rollNo)
        {
            _model.SetRollNo(rollNo);
        }

        public string GetStudentRollNo()
        {
            return _model.GetRollNo();
        }

        public void UpdateView()
        {
            _view.PrintStudentDetails(_model.GetName(), _model.GetRollNo());
        }
    }
    //使用 StudentController 方法来演示 MVC 设计模式的用法。
    public void Main()
    {
        //从数据库获取学生记录
        Student model = RetrieveStudentFromDatabase();
        //创建一个视图：把学生详细信息输出到控制台
        StudentView view = new StudentView();

        StudentController controller = new StudentController(model, view);

        controller.UpdateView();

        //更新模型数据
        controller.SetStudentName("John");
        controller.UpdateView();
    }

    public Student RetrieveStudentFromDatabase()
    {
        Student student = new Student();
        student.SetName("Robert");
        student.SetRollNo("10");
        return student;
    }
}
