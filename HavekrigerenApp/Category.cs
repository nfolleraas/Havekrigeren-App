using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp;

public class Category
{
    public string[] categories = new string[10];

    public Category()
    {
        categories[0] = "Beplantning";
        categories[1] = "Græsplæne";
        categories[2] = "Træfældning";
        categories[3] = "Træterrasse";
        categories[4] = "Udgravning";
        categories[5] = "Bortkørsel af jord";
    }
}
