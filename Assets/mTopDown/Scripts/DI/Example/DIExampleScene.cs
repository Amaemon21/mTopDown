using UnityEngine;

namespace DI
{
    public class DIExampleScene : MonoBehaviour 
    { 
        public void Init(DIContainer projectContainer)
        {
            DIContainer sceneContainer = new DIContainer(projectContainer);
            sceneContainer.RegisterSingleton(c => new MySceneService(c.Resolve<MyAwesomeProjectService>()));
            sceneContainer.RegisterSingleton(_ => new MyAwesomeFactory());
            sceneContainer.RegisterInstance(new MyAwesomeObject("instance", 10));

            MyAwesomeFactory objectFactory = sceneContainer.Resolve<MyAwesomeFactory>();

            for (int i = 0; i < 3; i++)
            {
                string id = $"o{i}";
                MyAwesomeObject o = objectFactory.CreateInstance(id, i);
                Debug.Log($"Object created with factory.\n{o}");
            }

            MyAwesomeObject instacne = sceneContainer.Resolve<MyAwesomeObject>();
            Debug.Log($"Object instance. /n{instacne}");
        }
    }
}