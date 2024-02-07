using AlloyCMS12.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace AlloyCMS12.Models.Pages
{
    /// <summary>
    /// Used to present a single product
    /// </summary>
    [SiteContentType(
        GUID = "17583DCD-3C11-49DD-A66D-0DEF0DD601FC",
        GroupName = Globals.GroupNames.Products)]
    [SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-product.png")]
    [AvailableContentTypes(
        Availability = Availability.Specific,
        IncludeOn = new[] { typeof(StartPage) })]
    public class ProductPage : StandardPage, IHasRelatedContent
    {
        [Required]
        [Display(Order = 305)]
        [UIHint(Globals.SiteUIHints.StringsCollection)]
        [CultureSpecific]
        public virtual IList<string> UniqueSellingPoints { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 330)]
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(IContentData) }, new[] { typeof(JumbotronBlock) })]
        public virtual ContentArea RelatedContentArea { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 340)]
        public virtual string ProductName { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 350)]
        public virtual string SearchName { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 360)]
        public virtual int Price { get; set; }
    }
}