using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Utils
{
    public static class ContextUtils
    {
        public static T FromContext<T>(object? context) where T : class
        {
            if (context == null)
                throw new ArgumentNullException("context is null");
            if(context is not T)
                throw new ArgumentException($"context is not of type {typeof(T).Name}");

            return (T)context;

        }
   
    }
}
