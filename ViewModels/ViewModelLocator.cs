using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Daily_Helper.ViewModels
{
    public class ViewModelLocator
    {
        public static DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool),
            typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(UIElement element)
        {
            return (bool)element.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(UIElement element, bool value)
        {
            element.SetValue(AutoWireViewModelProperty, value);
        }

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                Bind(d);
        }

        private static void Bind(DependencyObject view)
        {
            if (view is FrameworkElement frameworkElement)
            {
                var viewModelType = FindViewModel(frameworkElement.GetType());
                //frameworkElement.DataContext = Activator.CreateInstance(viewModelType);
                if (viewModelType is null) throw new TypeInitializationException($"Cannot find ViewModel type for view", null);
                frameworkElement.DataContext = ActivatorUtilities.CreateInstance(App.Current.AppHost.Services, viewModelType);
            }
        }

        private static Type FindViewModel(Type viewType)
        {
            string viewName = string.Empty;
            if (viewType.FullName is null) throw new TypeInitializationException("Cannot find ViewModel type for view", null);

            if (viewType.FullName.EndsWith("Window"))
            {
                viewName = viewType.FullName
                    .Replace("Window", string.Empty)
                    .Replace(".Views.", ".ViewModels."); //set vm namespace
            }

            if (viewType.FullName.EndsWith("View"))
            {


                var nsNames = viewType.FullName.Split('.').ToList();
                nsNames[^1]= nsNames[^1].Replace("View", string.Empty);

                viewName = nsNames
                    .Aggregate((a, b) => $"{a}.{b}")
                    .Replace(".Dialogs", string.Empty) // ViewModels for dialogs are in same NS as all others
                    .Replace(".Views.", ".ViewModels.");

            }

            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName) ?? throw new TypeInitializationException("Cannot find ViewModel type for view", null);

            return viewModelType;
        }
    }
}

