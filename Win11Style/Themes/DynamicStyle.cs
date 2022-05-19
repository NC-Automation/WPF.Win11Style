using System;
using System.Collections.Generic;
using System.Windows;

namespace Win11Style.Themes
{
    public class DynamicStyle
    {
        public static Style GetBaseStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(BaseStyleProperty);
        }

        public static void SetBaseStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(BaseStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for BaseStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseStyleProperty =
            DependencyProperty.RegisterAttached("BaseStyle", typeof(Style), typeof(DynamicStyle), new UIPropertyMetadata(DynamicStyle.StylesChanged));

        public static Style GetDerivedStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(DerivedStyleProperty);
        }

        public static void SetDerivedStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(DerivedStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for DerivedStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DerivedStyleProperty =
            DependencyProperty.RegisterAttached("DerivedStyle", typeof(Style), typeof(DynamicStyle), new UIPropertyMetadata(DynamicStyle.StylesChanged));

        private static void StylesChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(target.GetType()))
                throw new InvalidCastException("Target must be FrameworkElement");

            var Element = (FrameworkElement)target;

            var Styles = new List<Style>();

            var BaseStyle = GetBaseStyle(target);

            if (BaseStyle != null)
                Styles.Add(BaseStyle);

            var DerivedStyle = GetDerivedStyle(target);

            if (DerivedStyle != null)
                Styles.Add(DerivedStyle);

            Element.Style = MergeStyles(Styles);
        }

        public static Style MergeStyles(ICollection<Style> Styles)
        {
            var NewStyle = new Style();

            foreach (var Style in Styles)
            {
                ExtractBaseStyles(Style, NewStyle);

                foreach (var Setter in Style.Setters)
                    NewStyle.Setters.Add(Setter);

                foreach (var Trigger in Style.Triggers)
                    NewStyle.Triggers.Add(Trigger);
            }

            return NewStyle;
        }

        private static void ExtractBaseStyles(Style style, Style newStyle)
        {
            if (newStyle == null) return;
            var baseStyle = style.BasedOn;
            if (baseStyle != null)
            {
                if (baseStyle.BasedOn != null) ExtractBaseStyles(baseStyle, newStyle);
                foreach (var Setter in baseStyle.Setters)
                    newStyle.Setters.Add(Setter);

                foreach (var Trigger in baseStyle.Triggers)
                    newStyle.Triggers.Add(Trigger);
            }
        }

    }
}
