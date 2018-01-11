using System;
using System.Collections.Generic;
using System.Linq;
using Converters.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using ViewModels;
using ViewModels.Menu;

namespace Converters
{
    public class DocParser
    {
        public UpdateMenuViewModel Parse(string fileName)
        {
            var document = new Document(fileName);
            var section = document.Sections[0];

            var lines = new List<DocLine>();
            foreach (Paragraph par in section.Paragraphs)
            {
                if (string.IsNullOrWhiteSpace(par.Text)) continue;
                var l = new DocLine()
                {
                    Text = par.Text.Trim(),
                    Number = lines.Count
                };
                lines.Add(l);
            }

            var titleItem = lines.FirstOrDefault(IsTitle);
            var title = titleItem == null ? string.Empty : titleItem.Text;
            var date = titleItem == null ? DateTime.Now : title.ExtractDateFromTitle();
            var priceItem = lines.FirstOrDefault(IsPrice);
            var price = priceItem == null ? 0 : priceItem.Text.ExtractPriceFromString();

            lines.Remove(titleItem);
            lines.Remove(priceItem);
            lines.Remove(lines.FirstOrDefault(IsMenu));

            List<MenuSectionViewModel> m = null;
            ExtractMenu(lines, ref m);
            var model = new UpdateMenuViewModel()
            {
                LunchDate = (date ?? DateTime.Now).ToString("YYYY-MM-DD"),
                Price = price,
               // Title = title,
                Sections = m,
                //FilePath = fileName
            };
            return model;
        }


        private static void ExtractMenu(List<DocLine> lines, ref List<MenuSectionViewModel>  menu)
        {
            if (lines.Count == 0) return;
            if (menu == null)
            {
                menu = new List<MenuSectionViewModel>();
            }

            var section = lines.FirstOrDefault(l => !IsMenuItem(l));
            if (section == null) return;
            var nextSextion = lines.Where(l=> l != section).FirstOrDefault(l => !IsMenuItem(l));
            var menuSection = new MenuSectionViewModel()
            {
                Name = section.Text,
                Number = menu.Count + 1,
                Items = new List<MenuItemViewModel>()
            };
            var items = lines.Where(l => l.Number > section.Number);
            if (nextSextion != null)
            {
                items = items.Where(l => l.Number < nextSextion.Number).ToList();
            }

            menuSection.Items.AddRange(items.Select(i => new MenuItemViewModel()
            {
                Name = i.Text.RemoveNumber(),
                Number = menuSection.Items.Count() + 1
            }));
            lines.Remove(section);
            lines.RemoveRange(0, items.Count());
            menu.Add(menuSection);
            if (lines.Any())
            {
                ExtractMenu(lines, ref menu);
            }
        }


        private static bool IsMenuItem(DocLine par)
        {
            return par.Text.IsLineWithNum();
        }


        private static bool IsTitle(DocLine par)
        {
            const string checkedString = "бизнес";
            return par.Text.Trim().ToLower().Contains(checkedString);
        }

        private static bool IsMenu(DocLine par)
        {
            const string checkedString = "меню";
            return par.Text.Trim().ToLower().Contains(checkedString);
        }

        private static bool IsPrice(DocLine par)
        {
            const string checkedString = "цена";
            return par.Text.Trim().ToLower().Contains(checkedString);
        }


    }
}
