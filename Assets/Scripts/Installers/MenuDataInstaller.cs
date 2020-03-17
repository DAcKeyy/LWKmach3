using LWT.Installers;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MenuDataInstaller", menuName = "Installers/MenuDataInstaller")]
public class MenuDataInstaller : ScriptableObjectInstaller<MenuDataInstaller>
{
    [SerializeField]
    private MusicLibriary musicLibriary;

    public override void InstallBindings()
    {
        Container.BindInstance(musicLibriary);
    }
}