using UnityEngine;
using Zenject;

namespace ArkhamMenu
{
    public class BindInstallerMono : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DeckBuildManager>().AsSingle();
        }
    }
}