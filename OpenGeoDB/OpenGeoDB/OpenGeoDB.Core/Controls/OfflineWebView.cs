using System;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Controls
{
    public class OfflineWebView : WebView
    {
        public static readonly BindableProperty OfflineContentProperty = BindableProperty.Create(
            nameof(OfflineContent), typeof(string), typeof(OfflineWebView), null, BindingMode.OneWay, null, OnOfflineContentChanged);

        public string OfflineContent
        {
            get { return (string) GetValue(OfflineContentProperty); }
            set { SetValue(OfflineContentProperty, value); }
        }

        public OfflineWebView()
        {
            Navigating += (sender, args) =>
                {
                    if (!args.Url.StartsWith("file", StringComparison.Ordinal))
                        args.Cancel = true;
                };
        }

        private static void OnOfflineContentChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            OfflineWebView webView = bindable as OfflineWebView;
            if (webView != null)
            {
                webView.Source = new HtmlWebViewSource
                    {
                        Html = newvalue.ToString()
                    };
            }
        }
    }
}