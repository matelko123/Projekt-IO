using System.Windows.Forms;

namespace Projekt_wlasciwy
{
    class ErrorController
    {
        public static void ThrowUserError(string Message = "Something went wrong.")
        {
            MessageBox.Show(Message + "Check Log", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ThrowUserInfo(string Message)
        {
            if(Message is null)
                return;

            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
