using System;
using System.Linq;
using System.Windows.Controls;

namespace Organizer
{
    ///Круговая диаграмма раходов по статьям и названиям
    /// <summary>
    /// Interaction logic for ExpensedDiagramControl.xaml
    /// </summary>
    public partial class ExpensedDiagramControl : UserControl
    {
        public ExpensedDiagramControl(DateTime start, DateTime end)
        {
            InitializeComponent();

            using (organizerEntities db = new organizerEntities())
            {
                var expenses = db.Article.OfType<Expenditure>().
                    Where(e=>e.DateTime>=start && e.DateTime<end).GroupBy(e => new { e.TypeId, e.NameId}).
                    Select(e => new {
                        FullName = e.FirstOrDefault().ExpenditureType.Type+":"+e.FirstOrDefault().ExpenditureName.Name,
                        Money = e.Sum(ex => ex.Summ) }).ToList();

                Expenses.ItemsSource = expenses;
            }
        }
    }
}
