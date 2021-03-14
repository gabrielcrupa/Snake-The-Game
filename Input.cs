using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        //Butoane tastatura
        private static Hashtable keyTable = new Hashtable();

        //Verificare apasare butoane tastatura
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }

            return (bool) keyTable[key];
        }

        //Detectare apasare butoane tastatura
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
