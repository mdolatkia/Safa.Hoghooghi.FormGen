using DataAccess;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRuleEngineTest
{
    public partial class frmImposeRules : Form
    {
        public frmImposeRules()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //Biz_Rule myrule = new Biz_Rule();
        //    //myrule.Condition = new TestCondition();
        //    //myrule.Action = new TestAction();


        //    //Biz_RuleSet myruleSet = new Biz_RuleSet(myrule);
        //    //  TestEntities.Customer customer = new TestEntities.Customer();
        //    MyProjectEntities context = new MyProjectEntities();
        //    foreach (Table table in context.Table)
        //        if (table != null)
        //        {
        //            //customer.Age = 4;
        //            Biz_RuleSet myruleSet = new Biz_RuleSet("RuleSet2");

        //            myruleSet.Execute(table, context);


        //        }
        //    context.SaveChanges();

        //}

        private void btnRuleEntityRelationships_Click(object sender, EventArgs e)
        {
            MyProjectEntities context = new MyProjectEntities();
            context.Configuration.LazyLoadingEnabled = true;
            var list = context.TableDrivedEntity;
            progressBar1.Maximum = list.Count();
            progressBar1.Value = 0;
            foreach (var entity in list)
            {
                progressBar1.Value++;
                //customer.Age = 4;
                Biz_RuleSet myruleSet = new Biz_RuleSet("RuleSet3");
                myruleSet.Execute(entity, context);
            }
            context.SaveChanges();
            MessageBox.Show("Operation is completed.");
        }

        private void btnRuleEntity_Click(object sender, EventArgs e)
        {
            MyProjectEntities context = new MyProjectEntities();
            context.Configuration.LazyLoadingEnabled = true;

            //customer.Age = 4;
            Biz_RuleSet myruleSet = new Biz_RuleSet("TableDrivedEntity_IsReference");
            myruleSet.ActionEvent += myruleSet_ActionEvent;
            myruleSet.Execute(context, txtDatabase.Text);

           context.SaveChanges();
            MessageBox.Show("Operation is completed.");
        }

        void myruleSet_ActionEvent(object sender, ActionInfoArg e)
        {
            progressBar1.Maximum = e.TotalEventsCount;
            progressBar1.Value = e.EventsNumber;
        }
    }
}
