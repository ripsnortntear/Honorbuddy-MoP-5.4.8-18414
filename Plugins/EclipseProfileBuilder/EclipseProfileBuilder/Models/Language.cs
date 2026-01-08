using ArachnidCreations;
using ArachnidCreations.DevTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.EclipsePlugins.Models
{
    //Normally this is done using a resource file but HB wont seem to compile when one is included.
    public static class LanguageCore
    {
        public static Language lang { get; set; }
        public static List<Translation> Translations = new List<Translation>();
        public static List<TranslationControls> Controls = new List<TranslationControls>();
        public static void LoadLanguages() 
        {
            DAL.DBFile = @"C:\Users\william.harris\Source\Workspaces\Eclipse\EclipseSkinBot\EclipseSkinBot\Data\EclipseWoWDB.edb";
            DataTable dt = DAL.LoadSL3Data("Select * from Translations where language = 2 ");
            foreach (DataRow row in dt.Rows)
            {
                Translation trans = (Translation)InstantObject.convertDataRowtoObject(new Translation(), row);
                Translations.Add(trans);
            }
            dt = DAL.LoadSL3Data("Select * from TranslationControls");
            foreach (DataRow row in dt.Rows)
            {
                TranslationControls cont = (TranslationControls)InstantObject.convertDataRowtoObject(new TranslationControls(), row);
                Controls.Add(cont);
            }
        }
        private static Random randomGen = new Random();
        public static Form PopulateControls(Form form){
            
            foreach (Control control in form.Controls){
                TranslationControls co = Controls.Where(c => c.Name == control.Name).FirstOrDefault();
                if (co != null)
                {
                    var trans = Translations.Where(t => t.language == (int)lang && t.groupid == co.Id).FirstOrDefault();
                    control.Text = trans.value;
                }
                if (control.HasChildren) ListControls(control);
            }
            return form;
        }
        public static void ListControls(Control mcontrol)
        {
            foreach (Control control in mcontrol.Controls)
            {

                TranslationControls co = Controls.Where(c => c.Name == control.Name).FirstOrDefault();
                if (co != null)
                {
                    var trans = Translations.Where(t => t.language == (int)lang && t.groupid == co.Id).FirstOrDefault();
                    control.Text = trans.value;
                }
                if (control.HasChildren) ListControls(control);
            }
        } 
    //    private static Control addChildren(Control control)
    //    {

    //        foreach (Control cont in control.Controls)
    //        {
    //            TranslationControls co = Controls.Where(c=>c.Name == cont.Name).FirstOrDefault();
    //            var trans = Translations.Where(t => t.language == (int)lang && t.groupid == co.Id).FirstOrDefault();
    //            if (cont.Name == "label5")
    //                Console.WriteLine("blag");
    //            if (trans != null) 
    //                cont.Text = trans.value;
    //            if (cont.HasChildren) addChildren(cont);
    //        }
    //        return control;
    //    }
    //    public static Form PopulateControls(Form form)
    //    {
    //        foreach (Control cont in form.Controls)
    //        {
    //            TranslationControls co = Controls.Where(c => c.Name == cont.Name).FirstOrDefault();
    //            if (co != null)
    //            {
    //                var trans = Translations.Where(t => t.language == (int)lang && t.groupid == co.Id).FirstOrDefault();
    //                if (trans != null) cont.Text = trans.value;
    //            }
    //            if (cont.HasChildren) addChildren(cont);
    //        }
    //        return form;
    //    }
    }
    
    public enum Language {
        English = 1,
        Chinese = 2
    }
}
