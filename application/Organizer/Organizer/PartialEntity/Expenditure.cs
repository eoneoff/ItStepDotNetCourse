using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Expenditure
    {
        public override Control GetEditControl()
        {
            ExpenditureEditControl edit = new ExpenditureEditControl();
            edit.DataContext = this;
            return edit;
        }

        public override decimal Money
        {
            get { return (decimal)Summ; }
        }

        public override int WindowHeight { get { return 380; } }

        public override string FullName
        {
            get
            {
                if (Id > 0)
                {
                    using (organizerEntities db = new organizerEntities())
                    {
                        if (db.Entry(this).State == System.Data.Entity.EntityState.Detached)
                            db.Article.Attach(this);

                        if (!db.Entry(this).Reference(e => e.ExpenditureType).IsLoaded)
                            db.Entry(this).Reference(e => e.ExpenditureType).Load();

                        if (!db.Entry(this).Reference(e => e.ExpenditureName).IsLoaded)
                            db.Entry(this).Reference(e => e.ExpenditureName).Load();
                    }

                    return $"{ExpenditureType.Type}: {ExpenditureName.Name}";
                }
                else
                    return "New Record";
            }
        }

        public override string Type
        {
            get { return "Expenditure"; }
        }
    }
}
