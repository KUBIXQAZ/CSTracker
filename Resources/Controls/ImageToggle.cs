using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSTracker.Resources.Controls
{
    public class ImageToggle : Button
    {
        private Image Image;

        public string Source
        {
            get
            {
                return (string)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }
        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(
            "Source",
            typeof(string),
            typeof(ImageToggle),
            new PropertyMetadata(default(string), OnSourcePropertyChanged));

        public bool Toggled
        {
            get
            {
                return (bool)GetValue(ToggledProperty);
            }
            set
            {
                SetValue(ToggledProperty, value);
            }
        }
        public static readonly DependencyProperty ToggledProperty =
        DependencyProperty.Register(
            "Toggled",
            typeof(bool),
            typeof(ImageToggle),
            new PropertyMetadata(false, OnToggledPropertyChanged));

        public ImageToggle()
        {
            Style = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter(BackgroundProperty, Brushes.Transparent),
                    new Setter(BorderBrushProperty, Brushes.Transparent),
                    new Setter(BorderThicknessProperty, new Thickness(0)),
                    new Setter(CursorProperty, Cursors.Hand),
                    new Setter(TemplateProperty, new ControlTemplate(typeof(Button))
                    {
                        VisualTree = new FrameworkElementFactory(typeof(ContentPresenter))
                    })
                }
            };

            Image = new Image
            {
                Stretch = Stretch.Uniform
            };

            Content = Image;
        }

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageToggle;

            if(control != null)
            {
                if (control.Source != null) control.Image.Source = new BitmapImage(new Uri(control.Source, UriKind.Relative));
            }
        }

        private static void OnToggledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}