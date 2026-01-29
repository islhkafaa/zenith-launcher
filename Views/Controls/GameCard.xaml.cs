using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using System;
using System.Numerics;
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

        public event EventHandler<Game>? LaunchRequested;
        public event EventHandler<Game>? EditRequested;
        public event EventHandler<Game>? DeleteRequested;

        private Visual? _rootVisual;
        private Compositor? _compositor;

        public GameCard()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _rootVisual = ElementCompositionPreview.GetElementVisual(RootGrid);
            _compositor = _rootVisual.Compositor;
        }

        public string FormatPlayTime(TimeSpan playTime)
        {
            if (playTime.TotalHours < 1)
            {
                return $"{(int)playTime.TotalMinutes}m played";
            }
            return $"{(int)playTime.TotalHours}h played";
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_compositor == null || _rootVisual == null) return;

            var scaleAnimation = _compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1.0f, new Vector3(1.05f, 1.05f, 1.0f));
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(200);

            _rootVisual.CenterPoint = new Vector3((float)RootGrid.ActualWidth / 2, (float)RootGrid.ActualHeight / 2, 0);
            _rootVisual.StartAnimation("Scale", scaleAnimation);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_compositor == null || _rootVisual == null) return;

            var scaleAnimation = _compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 1.0f));
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(200);

            _rootVisual.StartAnimation("Scale", scaleAnimation);
        }

        private void OnLaunchClick(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                LaunchRequested?.Invoke(this, Game);
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                EditRequested?.Invoke(this, Game);
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                DeleteRequested?.Invoke(this, Game);
            }
        }
    }
}
