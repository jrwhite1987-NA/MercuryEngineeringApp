using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MercuryEngApp.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class CustomSlider : UserControl
    {
        private int pointX = 17;
        private int pointY = 125;

        private const int VALUE_0 = 0;
        private const double DEFAULT_CUSTOM_VALUE = 0.0;
        private const double DEFAULT_CUSTOM_HEIGHT = 150;
        private const double DEFAULT_CUSTOM_WIDTH = 170;
        private const string DEFAULT_CUSTOM_BUTTOM_TEXT = "0";
        private const double DEFAULT_CUSTOM_MAX = 150;
        private const double DEFAULT_CUSTOM_MIN = 23;
        private const int DEFAULT_CUSTOM_INTERVAL = 25;
        private const double DEFAULT_CUSTOM_TICK_POS = 33.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSlider"/> class.
        /// </summary>
        public CustomSlider()
        {
            this.InitializeComponent();
            SpectrumScreenPoints = new Point(pointX, pointY);
        }

  
        /// <summary>
        /// The Button Value Font Size property
        /// </summary>
        public static readonly DependencyProperty ButtonValueFontSizeProperty =
            DependencyProperty.Register("ButtonValueFontSize", typeof(double), typeof(CustomSlider), new PropertyMetadata(Convert.ToDouble(11)));

        /// <summary>
        /// Gets or sets the Button Value FontSize.
        /// </summary>
        /// <value>The custom tick list.</value>
        public double ButtonValueFontSize
        {
            get { return (double)GetValue(ButtonValueFontSizeProperty); }
            set { SetValue(ButtonValueFontSizeProperty, value); }
        }

        /// <summary>
        /// The custom tick list property
        /// </summary>
        public static readonly DependencyProperty CustomTickListProperty =
            DependencyProperty.Register("CustomTickList", typeof(List<string>), typeof(CustomSlider), new PropertyMetadata(new List<string>()));

        /// <summary>
        /// Gets or sets the custom tick list.
        /// </summary>
        /// <value>The custom tick list.</value>
        public List<string> CustomTickList
        {
            get { return (List<string>)GetValue(CustomTickListProperty); }
            set { SetValue(CustomTickListProperty, value); }
        }

        /// <summary>
        /// The Horizontal Image Source property
        /// </summary>
        public static readonly DependencyProperty HorizontalImageSourceProperty =
            DependencyProperty.Register("HorizontalImageSource", typeof(string), typeof(CustomSlider), new PropertyMetadata(@"/Themes/NA_Blue_FulSize_Line.png"));

        /// <summary>
        /// Gets or sets the Horizontal Image Source.
        /// </summary>
        /// <value>The custom tick list.</value>
        public string HorizontalImageSource
        {
            get { return (string)GetValue(HorizontalImageSourceProperty); }
            set { SetValue(HorizontalImageSourceProperty, value); }
        }

        /// <summary>
        /// The custom value property
        /// </summary>
        public static readonly DependencyProperty CustomValueProperty = DependencyProperty.Register(
     "CustomValue", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_VALUE));

        /// <summary>
        /// The custom height property
        /// </summary>
        public static readonly DependencyProperty CustomHeightProperty = DependencyProperty.Register(
   "CustomHeight", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_HEIGHT));

        /// <summary>
        /// The custom width property
        /// </summary>
        public static readonly DependencyProperty CustomWidthProperty = DependencyProperty.Register(
   "CustomWidth", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_WIDTH));

        /// <summary>
        /// The custom button text property
        /// </summary>
        public static readonly DependencyProperty CustomButtonTextProperty = DependencyProperty.Register(
    "CustomButtonText", typeof(string), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_BUTTOM_TEXT));

        /// <summary>
        /// The custom button text unit property
        /// </summary>
        public static readonly DependencyProperty CustomButtonTextUnitProperty = DependencyProperty.Register(
    "CustomButtonTextUnit", typeof(string), typeof(CustomSlider), new PropertyMetadata(string.Empty));

        /// <summary>
        /// The custom tick frequency property
        /// </summary>
        public static readonly DependencyProperty CustomTickFrequencyProperty = DependencyProperty.Register(
   "CustomTickFrequency", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_VALUE));

        /// <summary>
        /// The custom maximum property
        /// </summary>
        public static readonly DependencyProperty CustomMaximumProperty = DependencyProperty.Register(
     "CustomMaximum", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_MAX));

        /// <summary>
        /// The custom minimum property
        /// </summary>
        public static readonly DependencyProperty CustomMinimumProperty = DependencyProperty.Register(
     "CustomMinimum", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_MIN));

        /// <summary>
        /// The custom tick placement property
        /// </summary>
        public static readonly DependencyProperty CustomTickPlacementProperty = DependencyProperty.Register(
     "CustomTickPlacement", typeof(TickPlacement), typeof(CustomSlider), new PropertyMetadata(TickPlacement.None));

        /// <summary>
        /// The custom vertical track rect visibility property
        /// </summary>
        public static readonly DependencyProperty CustomVerticalTrackRectVisibilityProperty = DependencyProperty.Register(
     "CustomVerticalTrackRectVisibility", typeof(Visibility), typeof(CustomSlider), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// The custom vertical decrease rect visibility property
        /// </summary>
        public static readonly DependencyProperty CustomVerticalDecreaseRectVisibilityProperty =
            DependencyProperty.Register("CustomVerticalDecreaseRectVisibility", typeof(Visibility), typeof(CustomSlider), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// The custom vertical track rect color property
        /// </summary>
        public static readonly DependencyProperty CustomVerticalTrackRectColorProperty =
            DependencyProperty.Register("CustomVerticalTrackRectColor", typeof(Brush), typeof(CustomSlider), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// The custom vertical decrease rect color property
        /// </summary>
        public static readonly DependencyProperty CustomVerticalDecreaseRectColorProperty =
            DependencyProperty.Register("CustomVerticalDecreaseRectColor", typeof(Brush), typeof(CustomSlider), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// The is thumb enabled property
        /// </summary>
        public static readonly DependencyProperty IsThumbEnabledProperty =
            DependencyProperty.Register("IsThumbEnabled", typeof(Visibility), typeof(CustomSlider), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// The is thumb enabled property
        /// </summary>
        public static readonly DependencyProperty IsHorizontalLineEnabledProperty =
            DependencyProperty.Register("IsHorizontalLineEnabled", typeof(Visibility), typeof(CustomSlider), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets or sets the is Horizontal Line enabled.
        /// </summary>
        /// <value>The is thumb enabled.</value>
        public Visibility IsHorizontalLineEnabled
        {
            get { return (Visibility)GetValue(IsHorizontalLineEnabledProperty); }
            set { SetValue(IsHorizontalLineEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the custom vertical decrease rect.
        /// </summary>
        /// <value>The color of the custom vertical decrease rect.</value>
        public Brush CustomVerticalDecreaseRectColor
        {
            get { return (Brush)GetValue(CustomVerticalTrackRectColorProperty); }
            set { SetValue(CustomVerticalTrackRectColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the custom vertical track rect.
        /// </summary>
        /// <value>The color of the custom vertical track rect.</value>
        public Brush CustomVerticalTrackRectColor
        {
            get { return (Brush)GetValue(CustomVerticalTrackRectColorProperty); }
            set { SetValue(CustomVerticalTrackRectColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom vertical decrease rect visibility.
        /// </summary>
        /// <value>The custom vertical decrease rect visibility.</value>
        public Visibility CustomVerticalDecreaseRectVisibility
        {
            get { return (Visibility)GetValue(CustomVerticalDecreaseRectVisibilityProperty); }
            set { SetValue(CustomVerticalDecreaseRectVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the is thumb enabled.
        /// </summary>
        /// <value>The is thumb enabled.</value>
        public Visibility IsThumbEnabled
        {
            get { return (Visibility)GetValue(IsThumbEnabledProperty); }
            set { SetValue(IsThumbEnabledProperty, value); }
        }

        /// <summary>
        /// The interval property
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_INTERVAL));

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// The interval property
        /// </summary>
        public static readonly DependencyProperty TickPositionProperty =
            DependencyProperty.Register("TickPosition", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_TICK_POS));

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public double TickPosition
        {
            get { return (double)GetValue(TickPositionProperty); }
            set { SetValue(TickPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom vertical track rect visibility.
        /// </summary>
        /// <value>The custom vertical track rect visibility.</value>
        public Visibility CustomVerticalTrackRectVisibility
        {
            get { return (Visibility)GetValue(CustomVerticalTrackRectVisibilityProperty); }
            set { SetValue(CustomVerticalTrackRectVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom tick placement.
        /// </summary>
        /// <value>The custom tick placement.</value>
        public TickPlacement CustomTickPlacement
        {
            get { return (TickPlacement)GetValue(CustomTickPlacementProperty); }
            set { SetValue(CustomTickPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom maximum.
        /// </summary>
        /// <value>The custom maximum.</value>
        public double CustomMaximum
        {
            get { return (double)GetValue(CustomMaximumProperty); }
            set { SetValue(CustomMaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom minimum.
        /// </summary>
        /// <value>The custom minimum.</value>
        public double CustomMinimum
        {
            get { return (double)GetValue(CustomMinimumProperty); }
            set { SetValue(CustomMinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom value.
        /// </summary>
        /// <value>The custom value.</value>
        public double CustomValue
        {
            get { return (double)GetValue(CustomValueProperty); }
            set { SetValue(CustomValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the custom.
        /// </summary>
        /// <value>The height of the custom.</value>
        public double CustomHeight
        {
            get { return (double)GetValue(CustomHeightProperty); }
            set { SetValue(CustomHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the custom.
        /// </summary>
        /// <value>The width of the custom.</value>
        public double CustomWidth
        {
            get { return (double)GetValue(CustomWidthProperty); }
            set { SetValue(CustomWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom button text unit.
        /// </summary>
        /// <value>The custom button text unit.</value>
        public string CustomButtonTextUnit
        {
            get { return (string)GetValue(CustomButtonTextUnitProperty); }
            set { SetValue(CustomButtonTextUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom button text.
        /// </summary>
        /// <value>The custom button text.</value>
        public string CustomButtonText
        {
            get { return (string)GetValue(CustomButtonTextProperty); }
            set { SetValue(CustomButtonTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom tick frequency.
        /// </summary>
        /// <value>The custom tick frequency.</value>
        public double CustomTickFrequency
        {
            get { return (double)GetValue(CustomTickFrequencyProperty); }
            set { SetValue(CustomTickFrequencyProperty, value); }
        }

        /// <summary>
        /// The scale enable property
        /// </summary>
        public static readonly DependencyProperty ScaleEnableProperty =
           DependencyProperty.Register("ScaleEnable", typeof(bool), typeof(CustomSlider), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether [scale enable].
        /// </summary>
        /// <value><c>true</c> if [scale enable]; otherwise, <c>false</c>.</value>
        public bool ScaleEnable
        {
            get { return (bool)GetValue(ScaleEnableProperty); }
            set { SetValue(ScaleEnableProperty, value); }
        }

        /// <summary>
        /// The scale for property
        /// </summary>
        public static readonly DependencyProperty ScaleForProperty =
           DependencyProperty.Register("ScaleFor", typeof(ScaleTypeEnum), typeof(CustomSlider), new PropertyMetadata(ScaleTypeEnum.None));

        /// <summary>
        /// Gets or sets the scale for.
        /// </summary>
        /// <value>The scale for.</value>
        public ScaleTypeEnum ScaleFor
        {
            get { return (ScaleTypeEnum)GetValue(ScaleForProperty); }
            set { SetValue(ScaleForProperty, value); }
        }

        /// <summary>
        /// The Spectrum Screen Points Property
        /// </summary>
        public static readonly DependencyProperty SpectrumScreenPointsProperty =
           DependencyProperty.Register("SpectrumScreenPoints", typeof(Point), typeof(CustomSlider), new PropertyMetadata(new Point(VALUE_0, VALUE_0)));

        /// <summary>
        /// Gets or sets the Spectrum Screen Points
        /// </summary>
        /// <value>The scale for.</value>
        public Point SpectrumScreenPoints
        {
            get { return (Point)GetValue(SpectrumScreenPointsProperty); }
            set { SetValue(SpectrumScreenPointsProperty, value); }
        }

        /// <summary>
        /// The Spectrum Screen Points Property
        /// </summary>
        public static readonly DependencyProperty CustomSliderValueProperty =
           DependencyProperty.Register("CustomSliderValue", typeof(double), typeof(CustomSlider), new PropertyMetadata(DEFAULT_CUSTOM_VALUE));

      
        /// <summary>
        /// Gets or sets the Spectrum Screen Points
        /// </summary>
        /// <value>The scale for.</value>
        public double CustomSliderValue
        {
            get { return (double)GetValue(CustomSliderValueProperty); }
            set { SetValue(CustomSliderValueProperty, value); }
        }


        private void VerticalThumb_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var thumb = sender as Thumb;
            RearrangeThumb(thumb);
        }
        public void RearrangeThumb(Thumb thumb)
        {
            Point screenCoords = thumb.TransformToVisual(parentGrid).Transform(new Point(0, 0));
            SpectrumScreenPoints = screenCoords;
            CustomSliderValue = CusomSlider.Value;
            //CreateScale(screenCoords);
        }

        public Thumb Thumb
        {
            get
            {
                return GetThumb(this) as Thumb;
            }
        }

        public Point ThumbCoordinates(Thumb thumb)
        {
            UIElement scaleCtl = null;
            foreach (var control in parentGrid.Children)
            {
                if(control.GetType() == typeof(Grid))
                {
                    scaleCtl = (UIElement)control;
                }
            }
            if (scaleCtl != null)
            {
                return thumb.TransformToVisual(scaleCtl).Transform(new Point(0, 0));
            }
            return new Point(0, 0);
        }

        public Slider VerticalSlider
        {
            get { return CusomSlider; }
        }

        private DependencyObject GetThumb(DependencyObject root)
        {
            if (root is Thumb)
            {
                return root;
            }

            DependencyObject thumb = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                thumb = GetThumb(VisualTreeHelper.GetChild(root, i));

                if (thumb is Thumb)
                {
                    return thumb;
                }
            }

            return thumb;
        }

        private void CusomSlider_DragLeave(object sender, DragEventArgs e)
        {

        }

        ///// <summary>
        ///// Creates the scale.
        ///// </summary>
        ///// <param name="screenCoords">The screen coords.</param>
        //public void CreateScale(Point screenCoords)
        //{
        //    if (ScaleEnable && ScaleFor != ScaleTypeEnum.None && ScaleFor != ScaleTypeEnum.CVR)
        //    {
        //        SpectrumScreenPoints = screenCoords;
        //        ScaleGenerator.CreateScale(new ScaleParameters
        //        {
        //            ParentControl = parentGrid,
        //            CustomSliderValue = CusomSlider.Value,
        //            ScaleType = ScaleFor,
        //            ScreenCoords = screenCoords,
        //            Interval = Interval,
        //            Maximum = CustomMaximum,
        //            Minimum = CustomMinimum,
        //            TickPosition = TickPosition,
        //        });
        //    }

        //    if (ScaleFor == ScaleTypeEnum.EditSpectrogram)
        //    {
        //        parentGrid.Height = CusomSlider.Height;

        //        this.InvalidateArrange();
        //    }
        //}

        ///// <summary>
        ///// Creates the scale.
        ///// </summary>
        ///// <param name="screenCoords">The screen coords.</param>
        //public void CreateScale(Point screenCoords, double customSliderValue)
        //{
        //    if (ScaleEnable && ScaleFor != ScaleTypeEnum.None && ScaleFor != ScaleTypeEnum.CVR)
        //    {
        //        ScaleGenerator.CreateScale(new ScaleParameters
        //        {
        //            ParentControl = parentGrid,
        //            CustomSliderValue = customSliderValue,
        //            ScaleType = ScaleFor,
        //            ScreenCoords = screenCoords,
        //            Interval = Interval,
        //            Maximum = CustomMaximum,
        //            Minimum = CustomMinimum,
        //            TickPosition = TickPosition,
        //        });
        //    }
        //}

        ///// <summary>
        ///// Creates the scale.
        ///// </summary>
        //public void CreateScale()
        //{
        //    ScaleGenerator.CreateScale(new ScaleParameters
        //    {
        //        ParentControl = parentGrid,
        //        CustomSliderValue = CusomSlider.Value,
        //        ScaleType = ScaleFor,
        //        ScreenCoords = new Point(),
        //        Interval = Interval,
        //        Maximum = CustomMaximum,
        //        Minimum = CustomMinimum,
        //        TickPosition = TickPosition,
        //    });
        //}

   
    }

    /// <summary>
    /// Enum ScaleType
    /// </summary>
    public enum ScaleTypeEnum
    {
        /// <summary>
        /// The m mode
        /// </summary>
        MMode,

        /// <summary>
        /// The trending mean
        /// </summary>
        TrendingMean,

        /// <summary>
        /// The trending pi
        /// </summary>
        TrendingPI,

        /// <summary>
        /// The CVR
        /// </summary>
        CVR,

        /// <summary>
        /// The spectrogram
        /// </summary>
        Spectrogram,

        /// <summary>
        /// The edit spectrogram
        /// </summary>
        EditSpectrogram,

        /// <summary>
        /// The Trending Horizontal time scale
        /// </summary>
        TrendingHorizontal,

        /// <summary>
        /// The MModeTestReview
        /// </summary>
        MModeTestReview,

        /// <summary>
        /// The none
        /// </summary>
        None
    }
}
