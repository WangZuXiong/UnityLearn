Shader "Hidden/Sphere_Front"
{
    SubShader
    {
        Tags {"Queue" = "Transparent"}
        Pass
        {
            Color(1,0,0,1)
        }
    }
}


/*

    //Background 是 1000，Geometry 是 2000，AlphaTest 是 2450，Transparent 是 3000，Overlay 是 4000


    摄像机 sphere cube
    -------------->
    
    实验结果
   
   =============================================
   cube Tags {"Queue" = "Geometry"}
   sphere Tags {"Queue" = "Geometry"}

   sphere >>> cube 先渲染sphere再渲染cube  从前向后渲染
    


   =============================================
   cube Tags {"Queue" = "Transparent"}
   sphere Tags {"Queue" = "Transparent"}

   cube >>> sphere 先渲染cube再渲染sphere  从后向前渲染


   ===============================================
   cube Tags {"Queue" = "Transparent"}
   sphere Tags {"Queue" = "Geometry"}
   
   cube >>> sphere  //从后向前渲染

   ===============================================
   cube Tags {"Queue" = "Geometry"}
   sphere Tags {"Queue" = "Transparent"}

   cube >>> sphere  //从后向前渲染

   ===============================================
   cube Tags {"Queue" = "Overlay"}
   sphere Tags {"Queue" = "Transparent"}

   sphere >>> cube//Overlay队列最后渲染
*/
