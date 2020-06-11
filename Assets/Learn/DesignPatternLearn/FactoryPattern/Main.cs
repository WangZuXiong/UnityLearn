using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pattern.factory
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            Factory factory = new Factory();
            Fish sharkFish = factory.GetFish("sharkFish");
            sharkFish.Swim();
        }
    }
}