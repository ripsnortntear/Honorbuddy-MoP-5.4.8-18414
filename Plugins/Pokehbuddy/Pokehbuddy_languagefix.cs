namespace Pokehbuddyplug
{
    public partial class Pokehbuddy
    {
        private string ConvertFromOldFile(string logic)
        {



            //Chinese, japanese, russian and other weird character users should delete this line :
            return logic.Replace('·', '@').Replace('*', '@').Replace('þ', '$');
            //(you will lose backwards compatibility with old logic files)





            //and leave this one alone!
            return logic;


        }

    }
}