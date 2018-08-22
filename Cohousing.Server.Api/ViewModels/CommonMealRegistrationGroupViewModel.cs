using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealRegistrationGroupViewModel
    {
        public int GroupNo { get; set; }
        public string Name { get; set; }
        public IImmutableList<CommonMealRegistrationViewModel> Registrations { get; set; }
    }
}