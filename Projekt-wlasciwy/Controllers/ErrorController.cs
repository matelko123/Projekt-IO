using System.Windows.Forms;

namespace Projekt_wlasciwy
{
    class ErrorController
    {
        public static void ThrowUserError(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ThrowUserInfo(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
