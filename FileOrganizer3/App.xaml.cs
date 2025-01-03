﻿using System.Windows;
using FileOrganizer3.ViewModels;
using FileOrganizer3.Views;
using Prism.Ioc;

namespace FileOrganizer3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SettingPage, SettingPageViewModel>();
            containerRegistry.RegisterDialog<InputPage, InputPageViewModel>();
            containerRegistry.RegisterDialog<FileCopyPage, FileCopyPageViewModel>();
        }
    }
}