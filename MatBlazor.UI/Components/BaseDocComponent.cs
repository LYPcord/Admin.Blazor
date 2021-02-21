using Microsoft.AspNetCore.Components;

namespace MatBlazor.UI.Components
{
    public class BaseDocComponent : ComponentBase
    {
        [Parameter]
        public bool Secondary { get; set; }
    }
}