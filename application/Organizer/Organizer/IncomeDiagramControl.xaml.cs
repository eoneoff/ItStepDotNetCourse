using System;
using System.Linq;
using System.Windows.Controls;

namespace Organizer
{
    ///Круговая диаграмма доходов
    /// <summary>
    /// Interaction logic for IncomeDiagramControl.xaml
    /// </summary>
    public partial class IncomeDiagramControl : UserControl
    {
        public IncomeDiagramControl(DateTime start, DateTime end)
        {
            InitializeComponent();

            using (organizerEntities db = new organizerEntities())
            {
                var income = db.Article.OfType<Income>().Where(i => i.DateTime >= start && i.DateTime < end).
                    GroupBy(i => i.SourceId).Select(i => new {FullName =i.FirstOrDefault().IncomeSource.Name, Money = i.Sum(inc=>inc.Summ) }).ToList();

                Income.ItemsSource = income;
            }
        }
    }
}
