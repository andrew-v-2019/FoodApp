using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Spire.Doc;
using Spire.Doc.Documents;
using ViewModels.Menu;
using Services.Extensions;
using Spire.Doc.Formatting;
using ViewModels;

namespace Converters
{
    public class DocConverter
    {
        public string ConvertToDoc(UpdateMenuViewModel model, FileInfo template)
        {
            var tmpl = new Document(template.FullName);
            var section = tmpl.Sections[0];
            foreach (Paragraph par in section.Paragraphs)
            {
                var lunchDateStr = model.LunchDate.ParseDate().ToString(LocalizationStrings.RusDateFormat);
                if (par.Text.Contains("{lunchDate}"))
                {
                    par.Text = par.Text.Replace("{lunchDate}", lunchDateStr);
                }
                if (par.Text.Contains("{price}"))
                {
                    var pr = model.Price!=null ? model.Price.ToString():"0" ;
                    par.Text = par.Text.Replace("{price}", pr);
                }
                var parSectionId = GetParagraphSectionId(par.Text, model);
                if (parSectionId.HasValue)
                {
                    var parSection = model.Sections.FirstOrDefault(s => s.MenuSectionId == parSectionId.Value);
                    if (parSection != null)
                    {
                        foreach (var menuItem in parSection.Items)
                        {
                            var menuItemMeta = $"{menuItem.Number}. {menuItem.Name}";
                            par.AppendBreak(BreakType.LineBreak);
                            var tr = par.AppendText(menuItemMeta);
                            var format = par.Items[0].CharacterFormat;
                            tr.CharacterFormat.FontSize = format.FontSize - 4;
                            tr.CharacterFormat.Bold = true;
                        }
                    }
                }
            }

            var str = new FileStream(template.Directory.FullName + "\\Test11.docx", FileMode.Create);

            tmpl.SaveToStream(str, FileFormat.Docx);
            str.Flush();
            str.Close();         
            str.Dispose();
            return template.Directory.FullName + "\\Test11.docx";
        }

        private static int? GetParagraphSectionId(string paragraphText, UpdateMenuViewModel model)
        {
            foreach (var sec in model.Sections)
            {
                if (paragraphText.Trim().ToLower().Contains(sec.Name.Trim().ToLower()))
                {
                    return sec.MenuSectionId;
                }
            }
            return null;
        }
    }
}
