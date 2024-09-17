﻿using System;
using FileOrganizer3.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingPageViewModel : BindableBase, IDialogAware
    {
        private string prefixText = "prefix";
        private AppSettings setting;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public string PrefixText { get => prefixText; set => SetProperty(ref prefixText, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            setting.PrefixText = PrefixText;
            setting.Save();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            setting = AppSettings.Load();
            PrefixText = setting.PrefixText;
        }
    }
}