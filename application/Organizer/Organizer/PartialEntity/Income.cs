using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Organizer
{
    public partial class Income
    {
        public override Control GetEditControl()
        {
            IncomeEditControl control = new IncomeEditControl();
            control.DataContext = this;
            return control;
        }

        public override decimal Money
        {
            get { return Summ; }
        }

        public override int WindowHeight { get { return 270; } }

        public override string FullName
        {
            get
            {
                if (Id > 0)
                {
                    using (organizerEntities db = new organizerEntities())
                    {
                        db.Article.Attach(this);

                        if (!db.Entry(this).Reference(i => i.IncomeSource).IsLoaded)
                        {
                            db.Entry(this).Reference(i => i.IncomeSource).Load();
                        }
                    }

                    return IncomeSource.Name;
                }
                else
                    return "New Income";
            }
        }

        public override string Type
        {
            get { return "Income"; }
        }
    }
}
