using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
    
       // Container.Bind<GridManager>().FromComponentInHierarchy().AsSingle();
       Container.Bind<MainMenuUIManager>().FromComponentInHierarchy().AsSingle();
       
       /*
        Container.Bind<SaveManager>()
            .FromNewComponentOnNewGameObject() // Yeni bir GameObject oluştur ve onun üzerine SaveManager ekle
            .AsSingle() // SaveManager'ı singleton olarak tanımla
            .OnInstantiated((InjectContext context, SaveManager saveManager) => 
            {
                // SaveManager objesini sahneler arası silinmez yapıyoruz
                DontDestroyOnLoad(saveManager.gameObject);
            })
            .NonLazy(); 
            */
    }
    
    
}
