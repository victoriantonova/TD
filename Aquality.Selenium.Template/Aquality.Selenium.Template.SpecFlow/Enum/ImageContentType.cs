using System.ComponentModel;

namespace Aquality.Selenium.Template.Tests.Enum
{
    enum ImageContentType
    {
        [Description("image/jpeg")]
        Jpeg,
        [Description("image/png")]
        Png,
        [Description("image/gif")]
        Gif
    }
}
