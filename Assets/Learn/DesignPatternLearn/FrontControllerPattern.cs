using UnityEngine;
/// <summary>
/// 前端控制器模式
/// </summary>
public class FrontControllerPattern : MonoBehaviour
{
    public class HomeView
    {
        public void Show()
        {
            Debug.Log("Displaying Home Page");
        }
    }

    public class StudentView
    {
        public void Show()
        {
            Debug.Log("Displaying Student Page");
        }
    }

    public class Dispatcher
    {
        public StudentView StudentView;
        public HomeView HomeView;

        public Dispatcher()
        {
            StudentView = new StudentView();
            HomeView = new HomeView();
        }

        public void Dispatch(string request)
        {
            if (request.Equals("STUDENT"))
            {
                StudentView.Show();
            }
            else
            {
                HomeView.Show();
            }
        }
    }

    public class FrontController
    {
        private Dispatcher _dispatcher;

        public FrontController()
        {
            _dispatcher = new Dispatcher();
        }

        public bool IsAuthenticUser()
        {
            Debug.Log("User is authenticated successfully");
            return true;
        }

        public void TrackRequest(string request)
        {
            Debug.Log("Page requested:" + request);
        }

        public void DispatherRequest(string request)
        {
            TrackRequest(request);
            if (IsAuthenticUser())
            {
                _dispatcher.Dispatch(request);
            }
        }
    }
    public void Main()
    {
        FrontController frontController = new FrontController();
        frontController.DispatherRequest("HOME");
        frontController.DispatherRequest("STUDENT");
    }
}