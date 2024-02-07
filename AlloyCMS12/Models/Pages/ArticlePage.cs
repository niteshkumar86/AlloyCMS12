using System.ComponentModel.DataAnnotations;

namespace AlloyCMS12.Models.Pages
{
    /// <summary>
    /// Used primarily for publishing news articles on the website
    /// </summary>
    [SiteContentType(
        GroupName = Globals.GroupNames.News,
        GUID = "AEECADF2-3E89-4117-ADEB-F8D43565D2F4")]
    [SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class ArticlePage : StandardPage
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            VisibleInMenu = false;
        }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 340)]
        public virtual string ArticleName { get; set; }

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