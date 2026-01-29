using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Views.Controls
{
    public sealed partial class GameCard : UserControl
    {
        public static readonly DependencyProperty GameProperty =
            DependencyProperty.Register(
                nameof(Game),
                typeof(Game),
                typeof(GameCard),
                new PropertyMetadata(null));

        public Game Game
        {
            get => (Game)GetValue(GameProperty);
            set => SetValue(GameProperty, value);
        }

        public GameCard()
        {
            InitializeComponent();
        }

        public string FormatPlayTime(TimeSpan playTime)
        {
            if (playTime.TotalHours < 1)
            {
                return $"{(int)playTime.TotalMinutes}m played";
            }
            return $"{(int)playTime.TotalHours}h played";
        }
    }
}
